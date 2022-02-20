using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SetBreakpointsResponseBody : ProtocolResponseBody
    {
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
