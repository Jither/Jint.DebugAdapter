namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Restarts a debug session. Clients should only call this request if the capability ‘supportsRestartRequest’ is true.
    /// </summary>
    /// <remarks>
    /// If the capability is missing or has the value false, a typical client will emulate ‘restart’
    /// by terminating the debug adapter first and then launching it anew.
    /// </remarks>
    public class RestartArguments : ProtocolArguments
    {
        /// <summary>
        /// The latest version of the 'launch' or 'attach' configuration.
        /// </summary>
        // TODO: This is either AttachArguments or LaunchArguments
        public object Arguments { get; set; }
    }
}
