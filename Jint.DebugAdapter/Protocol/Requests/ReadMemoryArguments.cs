namespace Jint.DebugAdapter.Protocol.Requests
{
    public class ReadMemoryArguments : ProtocolArguments
    {
        public string MemoryReference { get; set; }
        public long? Offset { get; set; }
        public int Count { get; set; }
    }
}
