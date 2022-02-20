namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ReadMemoryResponseBody : ProtocolResponseBody
    {
        public string Address { get; set; }
        public int? UnreadableBytes { get; set; }
        public string Data { get; set; }
    }
}
