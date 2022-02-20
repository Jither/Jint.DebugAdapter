namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ReadMemoryResponse : ProtocolResponseBody
    {
        public string Address { get; set; }
        public int? UnreadableBytes { get; set; }
        public string Data { get; set; }
    }
}
