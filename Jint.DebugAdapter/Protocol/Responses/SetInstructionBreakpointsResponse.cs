using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SetInstructionBreakpointsResponse : ProtocolResponseBody
    {
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
