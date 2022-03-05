using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Replaces all existing data breakpoints with new data breakpoints.
    /// </summary>
    /// <remarks>
    /// To clear all data breakpoints, specify an empty array.
    /// When a data breakpoint is hit, a ‘stopped’ event (with reason ‘data breakpoint’) is generated.
    /// Clients should only call this request if the capability ‘supportsDataBreakpoints’ is true.
    /// </remarks>
    public class SetDataBreakpointsArguments : ProtocolArguments
    {
        /// <summary>
        /// Information about the data breakpoints. The array elements correspond to the elements
        /// of the input argument 'breakpoints' array.
        /// </summary>
        public List<DataBreakpoint> Breakpoints { get; set; }
    }
}
