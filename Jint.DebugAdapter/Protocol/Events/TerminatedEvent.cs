namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that debugging of the debuggee has terminated. This does not mean that the debuggee itself has exited.
    /// </summary>
    public class TerminatedEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "terminated";

        /// <summary>
        /// A debug adapter may set 'restart' to true (or to an arbitrary object) to request that the front end restarts the session.
        /// </summary>
        /// <remarks>
        /// The value is not interpreted by the client and passed unmodified as an attribute '__restart' to the 'launch' and 'attach' requests.
        /// </remarks>
        public object Restart { get; set; }
    }
}
