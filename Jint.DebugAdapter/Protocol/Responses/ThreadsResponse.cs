namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘threads’ request.
    /// </summary>
    public class ThreadsResponse : ProtocolResponseBody
    {
        /// <param name="threads">All threads.</param>
        public ThreadsResponse(List<Types.Thread> threads)
        {
            Threads = threads;
        }

        /// <summary>
        /// All threads.
        /// </summary>
        public List<Types.Thread> Threads { get; set; }
    }
}
