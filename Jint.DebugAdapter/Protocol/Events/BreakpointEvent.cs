using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that some information about a breakpoint has changed.
    /// </summary>
    public class BreakpointEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "breakpoint";

        public BreakpointEvent(StringEnum<ChangeReason> reason, Breakpoint breakpoint)
        {
            Reason = reason;
            Breakpoint = breakpoint;
        }

        /// <summary>
        /// The reason for the event.
        /// </summary>
        public StringEnum<ChangeReason> Reason { get; set; }

        /// <summary>
        /// The 'id' attribute is used to find the target breakpoint
        /// and the other attributes are used as the new values.
        /// </summary>
        public Breakpoint Breakpoint { get; set; }
    }
}
