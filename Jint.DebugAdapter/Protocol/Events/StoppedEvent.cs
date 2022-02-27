using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that the execution of the debuggee has stopped due to some condition.
    /// This can be caused by a break point previously set, a stepping request has completed,
    /// by executing a debugger statement etc.
    /// </summary>
    public class StoppedEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "stopped";

        public StoppedEvent(StopReason reason, int threadId = 1)
        {
            Reason = reason;
            ThreadId = threadId;
        }

        /// <summary>
        /// The reason for the event.
        /// </summary>
        /// <remarks>For backward compatibility this string is shown in the UI if the 'description' attribute is
        /// missing (but it must not be translated).</remarks>
        public StopReason Reason { get; set; }

        /// <summary>
        /// The full reason for the event, e.g. 'Paused on exception'. This string is shown in the UI as is and
        /// must be translated.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The thread which was stopped.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// A value of true hints to the frontend that this event should not change the focus.
        /// </summary>
        public bool? PreserveFocusHint { get; set; }

        /// <summary>
        /// Additional information. E.g. if reason is 'exception', text contains the exception name.
        /// </summary>
        /// <remarks>This string is shown in the UI.</remarks>
        public string Text { get; set; }

        /// <summary>
        /// If 'allThreadsStopped' is true, a debug adapter can announce that all threads have stopped.
        /// </summary>
        /// <remarks>
        /// The client should use this information to enable that all threads can be expanded to access
        /// their stacktraces.
        /// 
        /// If the attribute is missing or false, only the thread with the given threadId can be expanded.
        /// </remarks>
        public bool? AllThreadsStopped { get; set; }

        /// <summary>
        /// Ids of the breakpoints that triggered the event.
        /// </summary>
        /// <remarks>
        /// In most cases there will be only a single breakpoint but here are some examples for multiple breakpoints:
        /// - Different types of breakpoints map to the same location.
        /// - Multiple source breakpoints get collapsed to the same instruction by the compiler/runtime.
        /// - Multiple function breakpoints with different function names map to the same location.
        /// </remarks>
        public List<int> HitBreakpointIds { get; set; }
    }
}
