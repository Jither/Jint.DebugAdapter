namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class WriteMemoryResponse : ProtocolResponseBody
    {
        public long? Offset { get; set; }
        public int? BytesWritten { get; set; }
    }
}
