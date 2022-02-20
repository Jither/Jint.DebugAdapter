using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class StackTraceArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public int? StartFrame { get; set; }
        public int? Levels { get; set; }
        public StackFrameFormat Format { get; set; }
    }


}
