namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The ‘disconnect’ request is sent from the client to the debug adapter in order to stop debugging.
    /// </summary>
    /// <remarks>
    /// It asks the debug adapter to disconnect from the debuggee and to terminate the debug adapter.
    ///     
    /// If the debuggee has been started with the ‘launch’ request, the ‘disconnect’ request terminates the debuggee.
    /// If the ‘attach’ request was used to connect to the debuggee, ‘disconnect’ does not terminate the debuggee.
    /// 
    /// This behavior can be controlled with the ‘terminateDebuggee’ argument (if supported by the debug adapter).
    /// </remarks>
    public class DisconnectArguments : ProtocolArguments
    {
        /// <summary>
        /// A value of true indicates that this 'disconnect' request is part of a restart sequence.
        /// </summary>
        public bool? Restart { get; set; }

        /// <summary>
        /// Indicates whether the debuggee should be terminated when the debugger is disconnected.
        /// </summary>
        /// <remarks>
        /// If unspecified, the debug adapter is free to do whatever it thinks is best.
        /// The attribute is only honored by a debug adapter if the capability 'supportTerminateDebuggee' is true.
        /// </remarks>
        public bool? TerminateDebuggee { get; set; }

        /// <summary>
        /// Indicates whether the debuggee should stay suspended when the debugger is disconnected.
        /// </summary>
        /// <remarks>
        /// If unspecified, the debuggee should resume execution.
        /// The attribute is only honored by a debug adapter if the capability 'supportSuspendDebuggee' is true.
        /// </remarks>
        public bool? SuspendDebuggee { get; set; }
    }
}
