using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class BreakpointLocationsResponseBody : ProtocolResponseBody
    {
        public List<BreakpointLocation> Breakpoints { get; set; }
    }
}
