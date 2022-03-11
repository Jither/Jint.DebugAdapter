using System.Text.Encodings.Web;
using System.Text.Json;
using Jint.Native;
using Jint.Native.Argument;
using Jint.Native.Array;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Native.RegExp;
using Jint.Runtime.Debugger;
using Jint.Runtime.Descriptors;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public class ValueInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int VariablesReference { get; set; }
        public VariablePresentationHint PresentationHint { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public string MemoryReference { get; set; }
    }

    public class VariableStore
    {
        private static readonly JsonSerializerOptions stringToJsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly Engine engine;
        private int nextId = 1;
        private readonly Dictionary<int, VariableContainer> containers = new();

        public VariableStore(Engine engine)
        {
            this.engine = engine;
        }

        public int Add(DebugScope scope, CallFrame frame = null)
        {
            var container = new ScopeVariableContainer(this, nextId++, scope, frame);
            return Add(container);
        }

        public int Add(ObjectInstance instance)
        {
            var container = new ObjectVariableContainer(this, nextId++, instance);
            return Add(container);
        }

        public int Add(PropertyDescriptor prop, ObjectInstance owner)
        {
            var container = new PropertyVariableContainer(this, nextId++, prop, owner, engine);
            return Add(container);
        }

        private int Add(VariableContainer container)
        {
            containers.Add(container.Id, container);
            return container.Id;
        }

        public VariableContainer GetContainer(int id)
        {
            return containers[id];
        }

        public ValueInfo CreateValue(string name, JsValue value)
        {
            string valueString = value switch
            {
                null => "null",
                FunctionInstance => name,
                RegExpInstance rx => rx.ToString(),
                ArgumentsInstance => "[...]",
                ArrayInstance => "[...]",
                ObjectInstance => "{...}",
                // DebugAdapter needs to return escaped string with surrounding quotes
                JsString => JsonSerializer.Serialize(value.ToString(), stringToJsonOptions),
                _ => value.ToString()
            };

            // TODO: result.Type
            var result = CreateValue(name, valueString);

            if (value is ObjectInstance obj)
            {
                result.VariablesReference = Add(obj);
                // TODO: result.NamedVariables
                // TODO: result.IndexedVariables
            }

            return result;
        }

        public ValueInfo CreateValue(string name, PropertyDescriptor prop, ObjectInstance owner)
        {
            if (prop.Get != null)
            {
                return new ValueInfo { 
                    Name = $"get {name}", 
                    Value = "(...)",
                    // Add a variable reference for lazy evaluation of the getter
                    VariablesReference = Add(prop, owner),
                    PresentationHint = new VariablePresentationHint { Lazy = true }
                };
            }
            else
            {
                return CreateValue(name, prop.Value);
            }
        }

        private ValueInfo CreateValue(string name, string value)
        {
            var result = new ValueInfo
            {
                Name = name,
                Value = value
            };

            return result;
        }

        public void Clear()
        {
            containers.Clear();
        }
    }
}
