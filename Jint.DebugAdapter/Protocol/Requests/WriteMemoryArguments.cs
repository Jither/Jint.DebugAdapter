namespace Jint.DebugAdapter.Protocol.Requests
{
    public class WriteMemoryArguments : ProtocolArguments
    {
        public string MemoryReference { get; set; }
        public long? Offset { get; set; }
        public bool? AllowPartial { get; set; }
        public string Data { get; set; }
    }
}
