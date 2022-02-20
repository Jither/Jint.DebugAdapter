using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class ScopesArguments : ProtocolArguments
    {
        public int FrameId { get; set; }
    }
}
