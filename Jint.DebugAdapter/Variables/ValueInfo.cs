using System.Text.Encodings.Web;
using System.Text.Json;
using Jint.Native.Object;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Variables
{
    public abstract class ValueInfo
    {
        public string Name { get; }
        public string Value { get; protected set; }
        public string Type { get; protected set; }
        public int VariablesReference { get; set; }
        public VariablePresentationHint PresentationHint { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public string MemoryReference { get; set; }
        public string EvaluateName { get; set; }

        protected ValueInfo(string name)
        {
            Name = name;
        }

        protected static string GetObjectType(ObjectInstance obj)
        {
            return obj.Get("constructor")?.Get("name")?.ToString() ?? "Object";
        }
    }
}
