using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class SetBreakpointsArguments : ProtocolArguments
    {
        public Source Source { get; set; }
        public List<SourceBreakpoint> Breakpoints { get; set; }
        public List<int> Lines { get; set; } // Deprecated
        public bool? SourceModified { get; set; }
    }
}
