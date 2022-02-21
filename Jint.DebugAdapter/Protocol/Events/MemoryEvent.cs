namespace Jint.DebugAdapter.Protocol.Events
{
    public class MemoryEvent : ProtocolEventBody
    {
        public string MemoryReference { get; set; }
        public long Offset { get; set; }
        public long Count { get; set; }

        protected override string EventNameInternal => "memory";
    }
}
