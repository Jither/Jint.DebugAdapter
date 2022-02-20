using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class VariablesArguments : ProtocolArguments
    {
        public int VariablesReference { get; set; }
        public string Filter { get; set; }
        public int? Start { get; set; }
        public int? Count { get; set; }
        public ValueFormat Format { get; set; }
    }
}
