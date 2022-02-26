namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request returns the variable scopes for a given stackframe ID.
    /// </summary>
    public class ScopesArguments : ProtocolArguments
    {
        /// <summary>
        /// Retrieve the scopes for this stackframe.
        /// </summary>
        public int FrameId { get; set; }
    }
}
