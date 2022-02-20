using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Events;
using Jint.DebugAdapter.Protocol.Requests;
using Jint.DebugAdapter.Protocol.Responses;

namespace Jint.DebugAdapter.Protocol
{

    internal interface IPendingRequest
    {
        public bool Cancelled { get; }
        public void Cancel();
        public ProtocolRequest Request { get; }
    }

    internal class DebugProtocol
    {
        private const int bufferSize = 4096;
        private static readonly Regex rxContentLength = new(@"^.*?Content-Length: (?<length>\d+)\r\n\r\n", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly ProtocolHandler handler;
        private readonly Stream inputStream;
        private readonly Stream outputStream;

        private Thread inputThread;
        private bool isRunning;
        private bool isHandlingError;
        private bool isSending;
        private bool _isQueueingEvents;
        private readonly ManualResetEvent waitForMessages = new(true);
        private readonly CancellationTokenSource cts = new();
        private CancellationToken CancellationToken => cts.Token;
        private readonly List<IPendingRequest> pendingRequests = new();
        private int dispatcherThreadId;
        private readonly object syncDispatcher = new();
        private readonly object syncOutput = new();

        private readonly UTF8Buffer data = new();
        private int nextMessageBodyLength = -1;
        private readonly Queue<byte[]> messageQueueOut = new();
        private readonly ConcurrentQueue<ProtocolEvent> eventQueue = new();

        private int _nextSeq = 0;

        public bool IsQueueingEvents
        {
            get
            {
                lock (syncDispatcher)
                {
                    return _isQueueingEvents;
                }
            }
            set
            {
                lock (syncDispatcher)
                {
                    _isQueueingEvents = value;
                }
            }
        }

        public DebugProtocol(ProtocolHandler handler, Stream inputStream, Stream outputStream)
        {
            this.handler = handler;
            handler.Protocol = this;
            this.inputStream = inputStream;
            this.outputStream = Stream.Synchronized(outputStream); // Make sure only one thread is writing to output at once
        }

        public void Start()
        {
            inputThread ??= new Thread(new ThreadStart(ReadInputThread)) { Name = "DebugAdapter Input" };
            isRunning = true;
            inputThread.Start();
        }

        private void ReadInputThread()
        {
            dispatcherThreadId = Environment.CurrentManagedThreadId;
            byte[] buffer = new byte[bufferSize];
            try
            {
                lock (syncDispatcher)
                {
                    while (!CancellationToken.IsCancellationRequested)
                    {
                        int bytesRead = inputStream.ReadAsync(buffer, 0, buffer.Length, CancellationToken).Result;
                        if (bytesRead == 0)
                        {
                            Stop();
                            return;
                        }

                        data.Append(buffer, bytesRead);

                        while (true)
                        {
                            if (nextMessageBodyLength < 0)
                            {
                                if (!ProcessMessageHeader())
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (!ProcessMessageBody())
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isRunning = false;
                HandleFatalError(ex);
            }
            isRunning = false;
        }

        private bool ProcessMessageHeader()
        {
            string input = data.Peek();
            if (input == String.Empty)
            {
                return false;
            }

            Match match = rxContentLength.Match(input);
            if (!match.Success)
            {
                return false;
            }

            nextMessageBodyLength = Convert.ToInt32(match.Groups["length"].Value, CultureInfo.InvariantCulture);
            data.Remove(match.Length);

            return true;
        }

        private bool ProcessMessageBody()
        {
            if (data.ByteLength < nextMessageBodyLength)
            {
                return false;
            }
            string json = data.Pop(nextMessageBodyLength);
            nextMessageBodyLength = -1;
            try
            {
                // Don't send events until message handling is done
                IsQueueingEvents = true;

                HandleMessage(json);
                return true;
            }
            finally
            {
                IsQueueingEvents = false;
            }
        }

        private void HandleFatalError(Exception ex)
        {
            lock (syncDispatcher)
            {
                if (isHandlingError)
                {
                    return;
                }
                isHandlingError = true;
            }
            Logger.Log($"Fatal error - stopping. {ex.Message}");
            Stop();
        }

        private void HandleMessage(string json)
        {
            ProtocolMessage message = JsonHelper.Deserialize<ProtocolMessage>(json);
            
            switch (message)
            {
                case BaseProtocolRequest request:
                    HandleRequest(request);
                    break;
                case BaseProtocolResponse response:
                    HandleResponse(response);
                    break;
                case BaseProtocolEvent evt:
                    HandleEvent(evt);
                    break;
            }
        }

        private void HandleRequest(BaseProtocolRequest request)
        {
            try
            {
                Logger.Log(JsonHelper.SerializeForOutput(request));
                bool disconnecting = request.Command == "disconnect";
                if (disconnecting)
                {
                    IsQueueingEvents = false;
                }
                var responseBody = handler.HandleRequest(request);

                BuildAndSendResponse(request, responseBody, true);

                if (disconnecting)
                {
                    Stop();
                }
            }
            catch (ProtocolException ex)
            {
                var error = new ErrorResponse(ex);
                BuildAndSendResponse(request, error, false, ex.Message);
            }
            finally
            {
                ProcessQueuedEvents();
            }
        }

        private void BuildAndSendResponse(BaseProtocolRequest request, ProtocolResponseBody body, bool success, string message = null)
        {
            var response = new ProtocolResponse(request.Command, request.Seq, success, body, message);
            SendMessage(response);
        }

        private void HandleResponse(BaseProtocolResponse response)
        {
            Logger.Log(JsonHelper.SerializeForOutput(response));
        }

        private void HandleEvent(BaseProtocolEvent evt)
        {
            Logger.Log(JsonHelper.SerializeForOutput(evt));
        }

        public void Stop(int timeout = 2000)
        {
            if (!waitForMessages.WaitOne(timeout))
            {
                Logger.Log("Timed out waiting for messages");
            }

            lock (cts)
            {
                cts.Cancel();
                try
                {
                    CancelAll();
                }
                catch (ProtocolException ex)
                {

                }
            }
        }

        public bool WaitForReader(int timeout = -1)
        {
            CheckThread();
            if (timeout > 0)
            {
                return inputThread.Join(timeout);
            }
            inputThread.Join();
            return true;
        }

        
        private void CheckThread([CallerMemberName]string caller = null)
        {
            if (Environment.CurrentManagedThreadId == dispatcherThreadId)
            {
                throw new InvalidOperationException($"{caller} not allowed on on dispatcher thread");
            }
        }

        private void CancelAll()
        {
            lock (pendingRequests)
            {
                var exceptionList = new List<Exception>();
                foreach (var pendingRequest in pendingRequests)
                {
                    try
                    {
                        pendingRequest.Cancel();
                    }
                    catch (Exception ex)
                    {
                        exceptionList.Add(ex);
                    }
                }

                if (exceptionList.Count > 0)
                {
                    throw new ProtocolException("Exceptions while cancelling requests", new AggregateException(exceptionList));
                }
            }
        }

        private void SendRequest(IPendingRequest request)
        {
            lock (pendingRequests)
            {
                if (!isRunning || CancellationToken.IsCancellationRequested)
                {
                    request.Cancel();
                }
                else
                {
                    pendingRequests.Add(request);
                    SendMessage(request.Request);
                }
            }
        }

        internal void SendEvent(ProtocolEvent evt)
        {
            if (IsQueueingEvents && evt.Body is not OutputEvent)
            {
                this.eventQueue.Enqueue(evt);
            }
            else
            {
                SendMessage(evt);
            }
        }

        private void SendMessage(ProtocolMessage message)
        {
            if (message.Seq == 0)
            {
                message.Seq = GenerateNextSeq();
            }
            Logger.Log(JsonHelper.SerializeForOutput(message));

            string json = JsonHelper.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            string contentLength = $"Content-Length: {bytes.Length}\r\n\r\n";
            var bytesHeader = Encoding.UTF8.GetBytes(contentLength);

            var buffer = new byte[bytes.Length + bytesHeader.Length];
            Array.Copy(bytesHeader, 0, buffer, 0, bytesHeader.Length);
            Array.Copy(bytes, 0, buffer, bytesHeader.Length, bytes.Length);

            lock (syncOutput)
            {
                messageQueueOut.Enqueue(buffer);
                if (isSending)
                {
                    return;
                }
                isSending = true;
                waitForMessages.Reset();
                Task.Run(() => SendQueuedMessages());
            }
        }

        private void ProcessQueuedEvents()
        {
            while (eventQueue.TryDequeue(out var result))
            {
                SendMessage(result);
            }
        }
        private void SendQueuedMessages()
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                byte[] buffer;
                lock (syncOutput)
                {
                    if (messageQueueOut.Count == 0)
                    {
                        isSending = false;
                        waitForMessages.Set();
                        return;
                    }
                    buffer = messageQueueOut.Dequeue();
                }
                try
                {
                    outputStream.Write(buffer, 0, buffer.Length);
                    outputStream.Flush();
                }
                catch (Exception ex)
                {
                    lock (syncOutput)
                    {
                        isSending = false;
                        waitForMessages.Set();
                    }
                    HandleFatalError(ex);
                    return;
                }
            }
            lock (syncOutput)
            {
                isSending = false;
                waitForMessages.Set();
            }
        }

        private int GenerateNextSeq()
        {
            return Interlocked.Increment(ref _nextSeq);
        }
    }
}
