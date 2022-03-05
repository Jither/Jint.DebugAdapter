using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
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
        /// <param name="breakpoints">Information about the breakpoints. The array elements are in the same order
        /// as the elements of the 'breakpoints' (or the deprecated 'lines') array in the arguments.</param>
        public SetBreakpointsResponse(IEnumerable<Breakpoint> breakpoints)
        {
            Breakpoints = breakpoints;
        }

        /// <summary>
        /// Information about the breakpoints.
        /// The array elements are in the same order as the elements of the
        /// 'breakpoints' (or the deprecated 'lines') array in the arguments.
        /// </summary>
        public IEnumerable<Breakpoint> Breakpoints { get; set; }
    }
}
