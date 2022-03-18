using System.Text.Encodings.Web;
using System.Text.Json;
using Jint.Native;
using Jint.Native.Argument;
using Jint.Native.Array;
using Jint.Native.Date;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Native.RegExp;
using Jint.Native.TypedArray;
using Jint.Runtime.Debugger;
using Jint.Runtime.Descriptors;

namespace Jint.DebugAdapter.Variables
{
    public class VariableStore
    {
        private readonly ValueInfoProvider infoProvider;
        private int nextId = 1;
        private readonly Dictionary<int, VariableContainer> containers = new();

        public VariableStore()
        {
            this.infoProvider = new ValueInfoProvider(this);
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

        public int AddArrayLike(ObjectInstance instance)
        {
            var container = new ArrayLikeVariableContainer(this, nextId++, instance);
            return Add(container);
        }

        public int Add(PropertyDescriptor prop, ObjectInstance owner)
        {
            var container = new PropertyVariableContainer(this, nextId++, prop, owner);
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
            return infoProvider.Create(name, value);
        }

        public ValueInfo CreateValue(string name, PropertyDescriptor prop, ObjectInstance owner)
        {
            return infoProvider.Create(name, prop, owner);
        }

        public ValueInfo SetValue(int variablesReference, string name, JsValue value)
        {
            var container = containers.GetValueOrDefault(variablesReference);
            if (container == null)
            {
                throw new VariableException($"Unknown parent variables reference: {variablesReference}");
            }

            var newValue = container.SetVariable(name, value);
            return CreateValue(name, newValue);
        }

        public void Clear()
        {
            containers.Clear();
        }
    }
}
