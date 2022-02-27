using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setInstructionBreakpoints’ request
    /// </summary>
    public class SetInstructionBreakpointsResponse : ProtocolResponseBody
    {
        /// <param name="breakpoints">Information about the breakpoints. The array elements correspond to the
        /// elements of the 'breakpoints' array.</param>
        public SetInstructionBreakpointsResponse(List<Breakpoint> breakpoints)
        {
            Breakpoints = breakpoints;
        }

        /// <summary>
        /// Information about the breakpoints. The array elements correspond to the
        /// elements of the 'breakpoints' array.
        /// </summary>
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
