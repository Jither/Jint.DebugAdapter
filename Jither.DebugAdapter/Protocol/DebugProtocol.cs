using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO.Pipelines;
using System.Text;
using System.Text.RegularExpressions;
using Jither.DebugAdapter.Helpers;
using Jither.DebugAdapter.Protocol.Events;
using Jither.DebugAdapter.Protocol.Responses;

namespace Jither.DebugAdapter.Protocol
{
    internal interface IPendingRequest
    {
        public bool Cancelled { get; }
        public void Cancel();
        public ProtocolRequest Request { get; }
    }

    public class DebugProtocol
    {
        private readonly Logger logger = LogManager.GetLogger();

        private static readonly Regex rxContentLength = new(@"^.*?Content-Length: (?<length>\d+)\r\n\r\n", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly Adapter adapter;
        private readonly Stream inputStream;
        private readonly Stream outputStream;

        private bool isRunning;
        private bool isHandlingError;
        private bool _isSending;
        private bool _isQueueingEvents;
        private readonly ManualResetEvent waitForMessages = new(true);
        private readonly CancellationTokenSource cts = new();
        private CancellationToken CancellationToken => cts.Token;
        private readonly List<IPendingRequest> pendingRequests = new();
        private readonly object syncDispatcher = new();
        private readonly object syncOutput = new();

        private readonly Queue<byte[]> messageQueueOut = new();
        private readonly ConcurrentQueue<ProtocolEvent> eventQueue = new();

        private int _nextSeq = 0;

        // syncOutput should be locked when accessing IsSending
        private bool IsSending
        {
            get => _isSending;
            set
            {
                _isSending = value;
                if (_isSending)
                {
                    waitForMessages.Reset();
                }
                else
                {
                    waitForMessages.Set();
                }
            }
        }

        private bool IsQueueingEvents
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

        public DebugProtocol(Adapter adapter, Stream inputStream, Stream outputStream)
        {
            this.adapter = adapter;
            this.inputStream = inputStream;
            this.outputStream = Stream.Synchronized(outputStream); // Make sure only one thread is writing to output at once
        }

        public async Task StartAsync()
        {
            var pipe = new Pipe();
            isRunning = true;
            Task writing = FillPipeAsync(pipe.Writer);
            Task reading = ReadPipeAsync(pipe.Reader);

            await Task.WhenAll(reading, writing);
            isRunning = false;
        }

        private async Task FillPipeAsync(PipeWriter writer)
        {
            const int minimumBufferSize = 512;

            while (true)
            {
                Memory<byte> buffer = writer.GetMemory(minimumBufferSize);
                try
                {
                    int bytesRead = await inputStream.ReadAsync(buffer, CancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    if (bytesRead == 0)
                    {
                        break;
                    }
                    writer.Advance(bytesRead);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    HandleFatalError(ex);
                    if (Debugger.IsAttached)
                    {
                        throw;
                    }
                }

                var result = await writer.FlushAsync(CancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            await writer.CompleteAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task ReadPipeAsync(PipeReader reader)
        {
            // In addition to body length, nextMessageBodyLength functions as state (reading header = -1, reading body = > -1)
            int nextMessageBodyLength = -1;
            while (true)
            {
                var result = await reader.ReadAsync(CancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                var buffer = result.Buffer;

                // Keep reading until we cannot parse a header or body from the current buffer
                while (true)
                {
                    if (nextMessageBodyLength < 0)
                    {
                        if (!TryReadHeader(ref buffer, out var header))
                        {
                            break;
                        }
                        nextMessageBodyLength = ProcessHeader(header);
                    }
                    else
                    {
                        if (!TryReadBody(ref buffer, nextMessageBodyLength, out var body))
                        {
                            break;
                        }
                        await ProcessBody(body);
                        nextMessageBodyLength = -1;
                    }
                }
                reader.AdvanceTo(buffer.Start, buffer.End);

                // Checking ReadResult.IsCompleted and exiting the reading logic before processing the buffer results in data loss.
                // Hence, we check it after reading logic
                if (result.IsCompleted)
                {
                    break;
                }
            }
            await reader.CompleteAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private bool TryReadHeader(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> header)
        {
            // We're assuming that the header is actually ASCII - no UTF-8 sequences (which might include a byte 0x0d that's not a CR)
            // Find next occurrence of CR
            var newlinePosition = buffer.PositionOf((byte)'\r');
            if (newlinePosition == null)
            {
                header = default;
                return false;
            }

            // Check that we have 4 bytes available starting from the CR
            var newlinesBuffer = buffer.Slice(newlinePosition.Value);
            if (newlinesBuffer.Length < 4)
            {
                header = default;
                return false;
            }

            newlinesBuffer = newlinesBuffer.Slice(0, 4);
            var newlinesSpan = newlinesBuffer.ToSpan();

            // Check that those 4 bytes are two CR+LF = end of header
            if (newlinesSpan[0] != (byte)'\r' || newlinesSpan[1] != (byte)'\n' || newlinesSpan[2] != (byte)'\r' || newlinesSpan[3] != (byte)'\n')
            {
                header = default;
                return false;
            }

            var headerEndPosition = buffer.GetPosition(4, newlinePosition.Value);
            header = buffer.Slice(0, headerEndPosition);
            buffer = buffer.Slice(headerEndPosition);
            return true;
        }

        private bool TryReadBody(ref ReadOnlySequence<byte> buffer, int length, out ReadOnlySequence<byte> header)
        {
            // For body, we simply need at least content-length bytes in the buffer
            if (buffer.Length < length)
            {
                header = default;
                return false;
            }

            header = buffer.Slice(0, length);
            buffer = buffer.Slice(length);
            return true;
        }

        private int ProcessHeader(ReadOnlySequence<byte> buffer)
        {
            string value = Encoding.UTF8.GetString(buffer);
            Match match = rxContentLength.Match(value);
            if (!match.Success)
            {
                throw new ProtocolException($"Expected content-length header, but found: {value}");
            }

            return Convert.ToInt32(match.Groups["length"].Value, CultureInfo.InvariantCulture);
        }

        private async Task<bool> ProcessBody(ReadOnlySequence<byte> buffer)
        {
            string json = Encoding.UTF8.GetString(buffer);
            try
            {
                // Don't send events until message handling is done
                IsQueueingEvents = true;

                await HandleMessage(json);
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
            logger.Error($"Fatal error - stopping. {ex.Message}");
            Stop();
        }

        private async Task HandleMessage(string json)
        {
            ProtocolMessage message = JsonHelper.Deserialize<ProtocolMessage>(json);
            
            switch (message)
            {
                case BaseProtocolRequest request:
                    await HandleRequest(request);
                    break;
                case BaseProtocolResponse response:
                    await HandleResponse(response);
                    break;
                case BaseProtocolEvent evt:
                    await HandleEvent(evt);
                    break;
            }
        }

        private async Task HandleRequest(BaseProtocolRequest request)
        {
            try
            {
                // Interpolation for lazy serialization
                logger.Log(LogLevel.Verbose, $"{JsonHelper.SerializeForOutput(request)}");
                bool disconnecting = request.Command == "disconnect";
                if (disconnecting)
                {
                    IsQueueingEvents = false;
                }
                var responseBody = await adapter.HandleRequest(request);

                BuildAndSendResponse(request, responseBody, true);

                if (disconnecting)
                {
                    Stop();
                }
            }
            catch (Exception ex)
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

        private Task HandleResponse(BaseProtocolResponse response)
        {
            // Interpolation for lazy serialization
            logger.Log(LogLevel.Verbose, $"{JsonHelper.SerializeForOutput(response)}");
            return Task.CompletedTask;
        }

        private Task HandleEvent(BaseProtocolEvent evt)
        {
            // Interpolation for lazy serialization
            logger.Log(LogLevel.Verbose, $"{JsonHelper.SerializeForOutput(evt)}");
            return Task.CompletedTask;
        }

        public void Stop(int timeout = 2000)
        {
            if (!waitForMessages.WaitOne(timeout))
            {
                logger.Warning("Timed out waiting for messages");
            }

            lock (cts)
            {
                cts.Cancel();
                try
                {
                    CancelAllPendingRequests();
                }
                catch (ProtocolException ex)
                {
                    logger.Error($"Exception during Stop: {ex.Message}");
                }
            }
        }

        private void CancelAllPendingRequests()
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
            // Interpolation for lazy serialization
            logger.Log(LogLevel.Verbose, $"{JsonHelper.SerializeForOutput(message)}");

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
                if (IsSending)
                {
                    return;
                }
                IsSending = true;
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
                        IsSending = false;
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
                        IsSending = false;
                    }
                    HandleFatalError(ex);
                    if (Debugger.IsAttached)
                    {
                        throw;
                    }
                    return;
                }
            }
            lock (syncOutput)
            {
                IsSending = false;
            }
        }

        private int GenerateNextSeq()
        {
            return Interlocked.Increment(ref _nextSeq);
        }
    }
}
