using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setBreakpoints’ request.
    /// </summary>
    /// <remarks>
    /// Returned is information about each breakpoint created by this request.
    /// This includes the actual code location and whether the breakpoint could be verified.
    /// </summary>
    public class SetBreakpointsResponse : ProtocolResponseBody
    {
        /// <summary>
        /// Information about the breakpoints.
        /// The array elements are in the same order as the elements of the
        /// 'breakpoints' (or the deprecated 'lines') array in the arguments.
        /// </summary>
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
