using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class DisconnectArguments : ProtocolArguments
    {
        public bool? Restart { get; set; }
        public bool? TerminateDebuggee { get; set; }
        public bool? SuspendDebuggee { get; set; }
    }
}
