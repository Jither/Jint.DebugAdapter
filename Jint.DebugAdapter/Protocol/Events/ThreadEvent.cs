using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ThreadEvent : ProtocolEventBody
    {
        public ThreadChangeReason Reason { get; set; }
        public int ThreadId { get; set; }

        protected override string EventNameInternal => "thread";
    }
}
