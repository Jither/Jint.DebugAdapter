namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request suspends the debuggee.
    /// </summary>
    /// <remarks>
    /// The debug adapter first sends the response and then a ‘stopped’ event (with reason ‘pause’)
    /// after the thread has been paused successfully.
    /// </remarks>
    public class PauseArguments : ProtocolArguments
    {
        /// <summary>
        /// Pause execution for this thread.
        /// </summary>
        public int ThreadId { get; set; }
    }
}
