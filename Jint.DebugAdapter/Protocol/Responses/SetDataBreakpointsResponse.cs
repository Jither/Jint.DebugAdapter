using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setDataBreakpoints’ request.
    /// </summary>
    public class SetDataBreakpointsResponse : ProtocolResponseBody
    {
        /// <summary>
        /// Information about the data breakpoints. The array elements correspond to
        /// the elements of the input argument 'breakpoints' array.
        /// </summary>
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
