using System.Linq;
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
        protected readonly VariableStore store;

        public int Id { get; }
        
        protected VariableContainer(VariableStore store, int id)
        {
            this.store = store;
            Id = id;
        }

        public IEnumerable<Variable> GetVariables(VariableFilter filter, int? start, int? count)
        {
            IEnumerable<Variable> result;
            if (filter == VariableFilter.Indexed)
            {
                result = GetIndexedVariables(start, count);
            }
            else if (filter == VariableFilter.Named)
            {
                result = GetNamedVariables(start, count);

            }
            else
            {
                result = GetAllVariables(start, count);
            }

            return result;
        }

        protected Variable CreateVariable(string name, JsValue value)
        {
            return CreateVariable(store.CreateValue(name, value));
        }

        protected Variable CreateVariable(string name, PropertyDescriptor prop, ObjectInstance owner)
        {
            return CreateVariable(store.CreateValue(name, prop, owner));
        }

        private Variable CreateVariable(ValueInfo valueInfo)
        {
            return new Variable(valueInfo.Name, valueInfo.Value)
            {
                Type = valueInfo.Type,
                VariablesReference = valueInfo.VariablesReference,
                PresentationHint = valueInfo.PresentationHint,
                IndexedVariables = valueInfo.IndexedVariables,
                NamedVariables = valueInfo.NamedVariables,
                MemoryReference = valueInfo.MemoryReference,
                EvaluateName = valueInfo.EvaluateName
            };
        }

        protected abstract IEnumerable<Variable> GetAllVariables(int? start, int? count);

        protected virtual IEnumerable<Variable> GetNamedVariables(int? start, int? count) =>
            throw new NotImplementedException("Named filtering not implemented for this variable type");

        protected virtual IEnumerable<Variable> GetIndexedVariables(int? start, int? count) =>
            throw new NotImplementedException("Indexed filtering not implemented for this variable type");
    }
}
