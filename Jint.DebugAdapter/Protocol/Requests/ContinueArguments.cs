using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class ContinueArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public bool? SingleThread { get; set; }
    }
}
