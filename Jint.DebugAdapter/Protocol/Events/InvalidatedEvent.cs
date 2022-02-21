namespace Jint.DebugAdapter.Protocol.Events
{
    public class InvalidatedEvent : ProtocolEventBody
    {
        // export type InvalidatedAreas = 'all' | 'stacks' | 'threads' | 'variables' | string
        public List<string> InvalidatedAreas { get; set; }
        public int? ThreadId { get; set; }
        public int? StackFrameId { get; set; }

        protected override string EventNameInternal => "invalidated";
    }
}
