using System.Text.Json.Serialization;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime.Descriptors;
using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Variables
{
    public class JintVariable : Variable
    {
        public JintVariable(string name, string value) : base(name, value)
        {
        }

        [JsonIgnore]
        public int SortOrder { get; set; }
    }

    public abstract class VariableContainer
    {
        protected readonly VariableStore store;

        public int Id { get; }

        protected VariableContainer(VariableStore store, int id)
        {
            this.store = store;
            Id = id;
        }

        public IEnumerable<JintVariable> GetVariables(VariableFilter filter, int? start, int? count)
        {
            IEnumerable<JintVariable> result;
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

            return result.OrderBy(v => v.SortOrder);
        }

        public abstract JsValue SetVariable(string name, JsValue value);

        protected JintVariable CreateVariable(string name, JsValue value)
        {
            return CreateVariable(store.CreateValue(name, value));
        }

        protected JintVariable CreateVariable(string name, PropertyDescriptor prop, ObjectInstance owner)
        {
            return CreateVariable(store.CreateValue(name, prop, owner));
        }

        private JintVariable CreateVariable(ValueInfo valueInfo)
        {
            return new JintVariable(valueInfo.Name, valueInfo.Value)
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

        protected abstract IEnumerable<JintVariable> GetAllVariables(int? start, int? count);

        protected virtual IEnumerable<JintVariable> GetNamedVariables(int? start, int? count) =>
            throw new NotImplementedException("Named filtering not implemented for this variable type");

        protected virtual IEnumerable<JintVariable> GetIndexedVariables(int? start, int? count) =>
            throw new NotImplementedException("Indexed filtering not implemented for this variable type");
    }
}
