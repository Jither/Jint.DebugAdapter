namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class WriteMemoryResponseBody : ProtocolResponseBody
    {
        public long? Offset { get; set; }
        public int? BytesWritten { get; set; }
    }
}
