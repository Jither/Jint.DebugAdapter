namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The ‘terminate’ request is sent from the client to the debug adapter in order to give the debuggee a chance for terminating itself.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsTerminateRequest’ is true.
    /// </remarks>
    public class TerminateArguments : ProtocolArguments
    {
        /// <summary>
        /// A value of true indicates that this 'terminate' request is part of a restart sequence.
        /// </summary>
        public bool? Restart { get; set; }
    }
}
