using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jint.Native;
using Jint.Native.Argument;
using Jint.Native.Array;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Native.RegExp;
using Jint.Runtime.Descriptors;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public abstract class VariableContainer
    {
        private static JsonSerializerOptions stringToJsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        protected readonly VariableStore store;

        public int Id { get; }
        
        protected VariableContainer(VariableStore store, int id)
        {
            this.store = store;
            Id = id;
        }

        public IEnumerable<Variable> GetVariables()
        {
            return InternalGetVariables();
        }

        protected Variable CreateVariable(string name, JsValue value)
        {
            string valueString = value switch
            {
                null => "null",
                FunctionInstance f => name,
                RegExpInstance rx => rx.ToString(),
                ArgumentsInstance => "[...]",
                ArrayInstance => "[...]",
                ObjectInstance => "{...}",
                // DebugAdapter needs to return escaped string with surrounding quotes
                JsString => JsonSerializer.Serialize(value.ToString(), stringToJsonOptions),
                _ => value.ToString()
            };

            var result = new Variable(name, valueString);

            if (value is ObjectInstance obj)
            {
                result.VariablesReference = store.Add(obj);
            }

            return result;
        }

        protected Variable CreateVariable(string name, PropertyDescriptor prop, ObjectInstance owner)
        {
            if (prop.Get != null)
            {
                var result = new Variable($"get {name}", "(...)");
                // Add a variable reference for lazy evaluation of the getter
                result.VariablesReference = store.Add(prop, owner);
                result.PresentationHint = new VariablePresentationHint { Lazy = true };
                return result;
            }
            else
            {
                return CreateVariable(name, prop.Value);
            }
        }

        protected abstract IEnumerable<Variable> InternalGetVariables();
    }
}
