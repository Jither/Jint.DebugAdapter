using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class SetDataBreakpointsArguments : ProtocolArguments
    {
        public List<DataBreakpoint> Breakpoints { get; set; }
    }
}
