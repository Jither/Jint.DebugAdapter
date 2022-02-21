using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class BreakpointLocationsResponse : ProtocolResponseBody
    {
        public List<BreakpointLocation> Breakpoints { get; set; }
    }
}
