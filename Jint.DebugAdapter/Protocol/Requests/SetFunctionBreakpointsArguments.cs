using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class SetFunctionBreakpointsArguments : ProtocolArguments
    {
        public List<FunctionBreakpoint> Breakpoints { get; set; }
    }
}
