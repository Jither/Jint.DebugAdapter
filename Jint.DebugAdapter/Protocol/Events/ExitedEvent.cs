namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that the debuggee has exited and returns its exit code.
    /// </summary>
    public class ExitedEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "exited";

        public ExitedEvent(int exitCode)
        {
            ExitCode = exitCode;
        }

        /// <summary>
        /// The exit code returned from the debuggee.
        /// </summary>
        public int ExitCode { get; set; }
    }
}
