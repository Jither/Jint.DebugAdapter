using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class BreakpointLocationsArguments : ProtocolArguments
    {
        public Source Source { get; set; }
        public int Line { get; set; }
        public int? Column { get; set; }
        public int? EndLine { get; set; }
        public int? EndColumn { get; set; }
    }
}
