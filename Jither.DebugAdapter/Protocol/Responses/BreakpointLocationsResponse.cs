using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘breakpointLocations’ request.
    /// </summary>
    /// <remarks>
    /// Contains possible locations for source breakpoints.
    /// </remarks>
    public class BreakpointLocationsResponse : ProtocolResponseBody
    {
        /// <param name="breakpoints">Sorted set of possible breakpoint locations.</param>
        public BreakpointLocationsResponse(IEnumerable<BreakpointLocation> breakpoints)
        {
            Breakpoints = breakpoints;
        }

        /// <summary>
        /// Sorted set of possible breakpoint locations.
        /// </summary>
        public IEnumerable<BreakpointLocation> Breakpoints { get; set; }
    }
}
