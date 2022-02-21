using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class BreakpointEvent : ProtocolEventBody
    {
        public StringEnum<ChangeReason> Reason { get; set; }
        public Breakpoint Breakpoint { get; set; }

        protected override string EventNameInternal => "breakpoint";
    }
}
