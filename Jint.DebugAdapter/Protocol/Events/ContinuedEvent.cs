using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ContinuedEvent : ProtocolEventBody
    {
        public int ThreadId { get; set; }
        public bool? AllThreadsContinued { get; set; }

        protected override string EventNameInternal => "continued";
    }
}
