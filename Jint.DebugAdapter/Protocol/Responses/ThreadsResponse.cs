namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘threads’ request.
    /// </summary>
    public class ThreadsResponse : ProtocolResponseBody
    {
        /// <param name="threads">All threads.</param>
        public ThreadsResponse(IEnumerable<Types.Thread> threads)
        {
            Threads = threads;
        }

        /// <summary>
        /// All threads.
        /// </summary>
        public IEnumerable<Types.Thread> Threads { get; set; }
    }
}
