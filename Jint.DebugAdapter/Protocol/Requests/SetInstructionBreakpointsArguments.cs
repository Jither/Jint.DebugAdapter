using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Replaces all existing instruction breakpoints. Typically, instruction breakpoints would be set from a diassembly window.
    /// </summary>
    /// <remarks>
    /// To clear all instruction breakpoints, specify an empty array.
    /// When an instruction breakpoint is hit, a ‘stopped’ event (with reason ‘instruction breakpoint’) is generated.
    /// Clients should only call this request if the capability ‘supportsInstructionBreakpoints’ is true.
    /// </remarks>
    public class SetInstructionBreakpointsArguments : ProtocolArguments
    {
        /// <summary>
        /// The instruction references of the breakpoints
        /// </summary>
        public List<InstructionBreakpoint> Breakpoints { get; set; }
    }
}
