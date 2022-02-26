namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request sets the location where the debuggee will continue to run.
    /// </summary>
    /// <remarks>
    /// This makes it possible to skip the execution of code or to executed code again.
    /// 
    /// The code between the current location and the goto target is not executed but skipped.
    /// 
    /// The debug adapter first sends the response and then a ‘stopped’ event with reason ‘goto’.
    /// 
    /// Clients should only call this request if the capability ‘supportsGotoTargetsRequest’ is true
    /// (because only then goto targets exist that can be passed as arguments).
    /// </remarks>
    public class GotoArguments : ProtocolArguments
    {
        /// <summary>
        /// Set the goto target for this thread.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// The location where the debuggee will continue to run.
        /// </summary>
        public int TargetId { get; set; }
    }
}
