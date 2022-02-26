namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request restarts execution of the specified stackframe.
    /// </summary>
    /// <remarks>
    /// The debug adapter first sends the response and then a ‘stopped’ event (with reason ‘restart’) after the
    /// restart has completed.
    /// 
    /// Clients should only call this request if the capability ‘supportsRestartFrame’ is true.
    /// </remarks>
    public class RestartFrameArguments : ProtocolArguments
    {
        /// <summary>
        /// Restart this stackframe.
        /// </summary>
        public int FrameId { get; set; }
    }
}
