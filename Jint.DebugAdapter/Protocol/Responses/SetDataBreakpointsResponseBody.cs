using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SetDataBreakpointsResponseBody : ProtocolResponseBody
    {
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
