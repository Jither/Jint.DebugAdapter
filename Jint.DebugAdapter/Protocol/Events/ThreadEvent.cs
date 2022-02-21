using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    public class ThreadEvent : ProtocolEventBody
    {
        public StringEnum<ThreadChangeReason> Reason { get; set; }
        public int ThreadId { get; set; }

        protected override string EventNameInternal => "thread";
    }
}
