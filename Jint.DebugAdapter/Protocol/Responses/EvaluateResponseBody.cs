using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class EvaluateResponseBody : ProtocolResponseBody
    {
        public string Result { get; set; }
        public string Type { get; set; }
        public VariablePresentationHint PresentationHint { get; set; }
        public int VariablesReference { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public string MemoryReference { get; set; }
    }
}
