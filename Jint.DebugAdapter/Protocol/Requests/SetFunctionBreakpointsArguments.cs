using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class SetFunctionBreakpointsArguments : ProtocolArguments
    {
        public List<FunctionBreakpoint> Breakpoints { get; set; }
    }
}
