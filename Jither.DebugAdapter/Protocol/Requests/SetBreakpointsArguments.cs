using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Sets multiple breakpoints for a single source and clears all previous breakpoints in that source.
    /// </summary>
    /// <remarks>
    /// To clear all breakpoint for a source, specify an empty array.
    /// When a breakpoint is hit, a ‘stopped’ event (with reason ‘breakpoint’) is generated.
    /// </remarks>
    public class SetBreakpointsArguments : ProtocolArguments
    {
        /// <summary>
        /// The source location of the breakpoints; either 'source.path' or 'source.reference' must be specified.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// The code locations of the breakpoints.
        /// </summary>
        public List<SourceBreakpoint> Breakpoints { get; set; }

        /// <summary>
        /// The code locations of the breakpoints.
        /// </summary>
        [Obsolete("Deprecated in favor of Breakpoints")]
        public List<int> Lines { get; set; }

        /// <summary>
        /// A value of true indicates that the underlying source has been modified which results in
        /// new breakpoint locations.
        /// </summary>
        public bool? SourceModified { get; set; }
    }
}
