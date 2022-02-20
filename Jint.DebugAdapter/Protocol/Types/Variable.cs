using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class Variable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public VariablePresentationHint PresentationHint { get; set; }
        public string EvaluateName { get; set; }
        public int VariablesReference { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public string MemoryReference { get; set; }
    }

    internal class VariablePresentationHint
    {
        public string Kind { get; set; }
        public List<string> Attributes { get; set; }
        public string Visibility { get; set; }
    }
}
