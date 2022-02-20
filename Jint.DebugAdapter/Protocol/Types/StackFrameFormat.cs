using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class StackFrameFormat
    {
        public bool? Parameters { get; set; }
        public bool? ParameterTypes { get; set; }
        public bool? ParameterNames { get; set; }
        public bool? ParameterValues { get; set; }
        public bool? Line { get; set; }
        public bool? Module { get; set; }
        public bool? IncludeAll { get; set; }
    }
}
