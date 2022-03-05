using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Replaces all existing function breakpoints with new function breakpoints.
    /// </summary>
    /// <remarks>
    /// To clear all function breakpoints, specify an empty array.
    /// When a function breakpoint is hit, a ‘stopped’ event (with reason ‘function breakpoint’) is generated.
    /// Clients should only call this request if the capability ‘supportsFunctionBreakpoints’ is true.
    /// </remarks>
    /// </summary>
    public class SetFunctionBreakpointsArguments : ProtocolArguments
    {
        /// <summary>
        /// The function names of the breakpoints.
        /// </summary>
        public List<FunctionBreakpoint> Breakpoints { get; set; }
    }
}
