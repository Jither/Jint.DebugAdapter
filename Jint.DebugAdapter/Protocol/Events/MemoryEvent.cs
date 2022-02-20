namespace Jint.DebugAdapter.Protocol.Events
{
    internal class MemoryEvent : ProtocolEventBody
    {
        public string MemoryReference { get; set; }
        public long Offset { get; set; }
        public long Count { get; set; }

        protected override string EventNameInternal => "memory";
    }
}
