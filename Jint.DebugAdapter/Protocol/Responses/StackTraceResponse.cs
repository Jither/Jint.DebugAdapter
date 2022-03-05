using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘stackTrace’ request.
    /// </summary>
    public class StackTraceResponse : ProtocolResponseBody
    {
        /// <param name="stackFrames">The frames of the stackframe.</param>
        public StackTraceResponse(IEnumerable<StackFrame> stackFrames)
        {
            StackFrames = stackFrames;
        }

        /// <summary>
        /// The frames of the stackframe.
        /// </summary>
        /// <remarks>
        /// If the array has length zero, there are no stackframes available.
        /// This means that there is no location information available.
        /// </remarks>
        public IEnumerable<StackFrame> StackFrames { get; set; }

        /// <summary>
        /// The total number of frames available in the stack.
        /// </summary>
        /// <remarks>
        /// If omitted or if totalFrames is larger than the available frames, a client is expected to request frames
        /// until a request returns less frames than requested (which indicates the end of the stack). Returning
        /// monotonically increasing totalFrames values for subsequent requests can be used to enforce paging in
        /// the client.
        /// </remarks>
        public int? TotalFrames { get; set; }
    }
}
