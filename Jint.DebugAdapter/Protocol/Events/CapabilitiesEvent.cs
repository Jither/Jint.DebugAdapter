using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class CapabilitiesEvent : ProtocolEventBody
    {
        public Capabilities Capabilities { get; set; }

        protected override string EventNameInternal => "capabilities";
    }
}
