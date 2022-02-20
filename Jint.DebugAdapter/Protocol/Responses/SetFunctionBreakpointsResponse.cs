using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SetFunctionBreakpointsResponse : ProtocolResponseBody
    {
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
