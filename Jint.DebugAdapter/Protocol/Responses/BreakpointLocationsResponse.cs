using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘breakpointLocations’ request.
    /// </summary>
    /// <remarks>
    /// Contains possible locations for source breakpoints.
    /// </remarks>
    public class BreakpointLocationsResponse : ProtocolResponseBody
    {
        public BreakpointLocationsResponse(List<BreakpointLocation> breakpoints)
        {
            Breakpoints = breakpoints;
        }

        /// <summary>
        /// Sorted set of possible breakpoint locations.
        /// </summary>
        public List<BreakpointLocation> Breakpoints { get; set; }
    }
}
