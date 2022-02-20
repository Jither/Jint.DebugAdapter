using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SetFunctionBreakpointsResponseBody : ProtocolResponseBody
    {
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
