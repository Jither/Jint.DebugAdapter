using Jither.DebugAdapter.Helpers;
using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that a thread has started or exited.
    /// </summary>
    public class ThreadEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "thread";

        public ThreadEvent(ThreadChangeReason reason, int threadId)
        {
            Reason = reason;
            ThreadId = threadId;
        }

        /// <summary>
        /// The reason for the event.
        /// </summary>
        public ThreadChangeReason Reason { get; set; }

        /// <summary>
        /// The identifier of the thread.
        /// </summary>
        public int ThreadId { get; set; }
    }
}
