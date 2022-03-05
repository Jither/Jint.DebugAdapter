using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setInstructionBreakpoints’ request
    /// </summary>
    public class SetInstructionBreakpointsResponse : ProtocolResponseBody
    {
        /// <param name="breakpoints">Information about the breakpoints. The array elements correspond to the
        /// elements of the 'breakpoints' array.</param>
        public SetInstructionBreakpointsResponse(IEnumerable<Breakpoint> breakpoints)
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
