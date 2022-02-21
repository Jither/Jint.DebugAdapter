using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class SetDataBreakpointsArguments : ProtocolArguments
    {
        public List<DataBreakpoint> Breakpoints { get; set; }
    }
}
