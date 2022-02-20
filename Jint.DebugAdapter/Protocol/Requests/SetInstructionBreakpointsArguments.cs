using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class SetInstructionBreakpointsArguments : ProtocolArguments
    {
        public List<InstructionBreakpoint> Breakpoints { get; set; }
    }
}
