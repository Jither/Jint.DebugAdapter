using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setFunctionBreakpoints’ request.
    /// Returned is information about each breakpoint created by this request.
    /// </summary>
    public class SetFunctionBreakpointsResponse : ProtocolResponseBody
    {
        /// <summary>
        /// Information about the breakpoints. The array elements correspond to the
        /// elements of the 'breakpoints' array.
        /// </summary>
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
