namespace Jither.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that the execution of the debuggee has continued.
    /// </summary>
    /// <remarks>
    /// Please note: a debug adapter is not expected to send this event in response to a request that implies that
    /// execution continues, e.g. ‘launch’ or ‘continue’. It is only necessary to send a ‘continued’ event if there
    /// was no previous request that implied this.
    /// </remarks>
    public class ContinuedEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "continued";

        public ContinuedEvent(int threadId = 1)
        {
            ThreadId = threadId;
        }

        /// <summary>
        /// The thread which was continued.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// If 'allThreadsContinued' is true, a debug adapter can announce that all threads have continued.
        /// </summary>
        public bool? AllThreadsContinued { get; set; }
    }
}
