namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘continue’ request.
    /// </summary>
    public class ContinueResponse : ProtocolResponseBody
    {
        /// <summary>
        /// The value true (or a missing property) signals to the client that all threads
        /// have been resumed.The value false must be returned if not all threads were resumed.
        /// </summary>
        public bool? AllThreadsContinued { get; set; }
    }
}
