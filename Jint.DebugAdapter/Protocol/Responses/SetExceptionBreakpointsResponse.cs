using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SetExceptionBreakpointsResponse : ProtocolResponseBody
    {
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
