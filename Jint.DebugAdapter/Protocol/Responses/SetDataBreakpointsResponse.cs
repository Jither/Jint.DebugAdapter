using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setDataBreakpoints’ request.
    /// </summary>
    public class SetDataBreakpointsResponse : ProtocolResponseBody
    {
        /// <param name="breakpoints">Information about the data breakpoints. The array elements correspond to
        /// the elements of the input argument 'breakpoints' array.</param>
        public SetDataBreakpointsResponse(IEnumerable<Breakpoint> breakpoints)
        {
            Breakpoints = breakpoints;
        }

        /// <summary>
        /// Information about the data breakpoints. The array elements correspond to
        /// the elements of the input argument 'breakpoints' array.
        /// </summary>
        public IEnumerable<Breakpoint> Breakpoints { get; set; }
    }
}
