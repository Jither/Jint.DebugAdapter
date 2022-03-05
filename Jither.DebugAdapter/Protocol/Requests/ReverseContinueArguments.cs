namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request resumes backward execution of all threads.
    /// </summary>
    /// <remarks>
    /// If the debug adapter supports single thread execution (see capability ‘supportsSingleThreadExecutionRequests’)
    /// setting the ‘singleThread’ argument to true resumes only the specified thread. If not all threads were resumed,
    /// the ‘allThreadsContinued’ attribute of the response must be set to false.
    ///
    /// Clients should only call this request if the capability ‘supportsStepBack’ is true.
    /// </remarks>
    public class ReverseContinueArguments : ProtocolArguments
    {
        /// <summary>
        /// Specifies the active thread. If the debug adapter supports single thread execution
        /// (see 'supportsSingleThreadExecutionRequests') and the optional argument 'singleThread' is true, 
        /// only the thread with this ID is resumed.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// If this optional flag is true, backward execution is resumed only for the thread with given 'threadId'.
        /// </summary>
        public bool? SingleThread { get; set; }
    }
}
