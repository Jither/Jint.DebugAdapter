using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class SetBreakpointsArguments : ProtocolArguments
    {
        public Source Source { get; set; }
        public List<SourceBreakpoint> Breakpoints { get; set; }
        public List<int> Lines { get; set; } // Deprecated
        public bool? SourceModified { get; set; }
    }
}
