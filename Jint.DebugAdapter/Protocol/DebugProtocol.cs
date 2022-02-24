﻿using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO.Pipelines;
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

    public class DebugProtocol
    {
        private static readonly Regex rxContentLength = new(@"^.*?Content-Length: (?<length>\d+)\r\n\r\n", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly Adapter adapter;
        private readonly Stream inputStream;
        private readonly Stream outputStream;

        private bool isRunning;
        private bool isHandlingError;
        private bool isSending;
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
                    int bytesRead = await inputStream.ReadAsync(buffer, CancellationToken);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    writer.Advance(bytesRead);
                }
                catch (Exception ex)
                {
                    HandleFatalError(ex);
                    if (Debugger.IsAttached)
                    {
                        throw;
                    }
                    break;
                }

                var result = await writer.FlushAsync(CancellationToken);
                if (result.IsCompleted)
                {
                    break;
                }
            }

            await writer.CompleteAsync();
        }

        private async Task ReadPipeAsync(PipeReader reader)
        {
            int nextMessageBodyLength = -1;
            while (true)
            {
                var result = await reader.ReadAsync(CancellationToken);
                var buffer = result.Buffer;
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
                        ProcessBody(body);
                        nextMessageBodyLength = -1;
                    }
                }
                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.IsCompleted)
                {
                    break;
                }
            }
            await reader.CompleteAsync();
        }

        private bool TryReadHeader(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> header)
        {
            var newlinePosition = buffer.PositionOf((byte)'\r');
            if (newlinePosition == null)
            {
                header = default;
                return false;
            }

            var newlinesBuffer = buffer.Slice(newlinePosition.Value);
            if (newlinesBuffer.Length < 4)
            {
                header = default;
                return false;
            }

            newlinesBuffer = newlinesBuffer.Slice(0, 4);
            var newlinesSpan = newlinesBuffer.ToSpan();
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

        private bool ProcessBody(ReadOnlySequence<byte> buffer)
        {
            string json = Encoding.UTF8.GetString(buffer);
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
                var responseBody = adapter.HandleRequest(request);

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
                    if (Debugger.IsAttached)
                    {
                        throw;
                    }
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