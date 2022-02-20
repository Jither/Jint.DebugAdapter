using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class BreakpointEvent : ProtocolEventBody
    {
        public ChangeReason Reason { get; set; }
        public Breakpoint Breakpoint { get; set; }

        protected override string EventNameInternal => "breakpoint";
    }
}
