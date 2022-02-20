namespace Jint.DebugAdapter.Protocol.Events
{
    internal class MemoryEventBody : ProtocolEventBody
    {
        public string MemoryReference { get; set; }
        public long Offset { get; set; }
        public long Count { get; set; }
    }
}
