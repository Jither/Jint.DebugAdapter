namespace Jint.DebugAdapter.Protocol.Events
{
    internal class InvalidatedEventBody : ProtocolEventBody
    {
        // export type InvalidatedAreas = 'all' | 'stacks' | 'threads' | 'variables' | string
        public List<string> InvalidatedAreas { get; set; }
        public int? ThreadId { get; set; }
        public int? StackFrameId { get; set; }
    }
}
