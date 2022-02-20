using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class NextArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public bool? SingleThread { get; set; }
        public SteppingGranularity? Granularity { get; set; }
    }
}
