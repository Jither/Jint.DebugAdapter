using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setFunctionBreakpoints’ request.
    /// Returned is information about each breakpoint created by this request.
    /// </summary>
    public class SetFunctionBreakpointsResponse : ProtocolResponseBody
    {
        /// <param name="breakpoints">Information about the breakpoints. The array elements correspond to the
        /// elements of the 'breakpoints' array.</param>
        public SetFunctionBreakpointsResponse(IEnumerable<Breakpoint> breakpoints)
        {
            Breakpoints = breakpoints;
        }

        /// <summary>
        /// Information about the breakpoints. The array elements correspond to the
        /// elements of the 'breakpoints' array.
        /// </summary>
        public IEnumerable<Breakpoint> Breakpoints { get; set; }
    }
}
