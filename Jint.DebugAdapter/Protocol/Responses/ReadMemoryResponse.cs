namespace Jint.DebugAdapter.Protocol.Responses
{
    public class ReadMemoryResponse : ProtocolResponseBody
    {
        public string Address { get; set; }
        public long? UnreadableBytes { get; set; }
        public string Data { get; set; }
    }
}
