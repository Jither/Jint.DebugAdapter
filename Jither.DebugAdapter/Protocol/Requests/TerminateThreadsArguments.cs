namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request terminates the threads with the given ids.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsTerminateThreadsRequest’ is true.
    /// </remarks>
    public class TerminateThreadsArguments : ProtocolArguments
    {
        /// <summary>
        /// Ids of threads to be terminated.
        /// </summary>
        public List<int> ThreadIds { get; set; }
    }
}
