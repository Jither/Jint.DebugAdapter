using System.Text.Encodings.Web;
using System.Text.Json;
using Jint.Native.Object;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Variables
{
    public class ValueInfo
    {
        public string Name { get; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int VariablesReference { get; set; }
        public VariablePresentationHint PresentationHint { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public string MemoryReference { get; set; }
        public string EvaluateName { get; set; }

        public ValueInfo(string name)
        {
            Name = name;
        }
    }
}
