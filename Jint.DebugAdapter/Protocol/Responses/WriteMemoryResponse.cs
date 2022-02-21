namespace Jint.DebugAdapter.Protocol.Responses
{
    public class WriteMemoryResponse : ProtocolResponseBody
    {
        public long? Offset { get; set; }
        public int? BytesWritten { get; set; }
    }
}
