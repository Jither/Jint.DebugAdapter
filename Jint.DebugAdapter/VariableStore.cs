using Jint.Native.Object;
using Jint.Runtime.Debugger;
using Jint.Runtime.Descriptors;

namespace Jint.DebugAdapter
{
    public class VariableStore
    {
        private readonly Engine engine;
        private int nextId = 1;
        private readonly Dictionary<int, VariableContainer> containers = new();

        public VariableStore(Engine engine)
        {
            this.engine = engine;
        }

        public int Add(DebugScope scope)
        {
            var container = new ScopeVariableContainer(this, nextId++, scope);
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

        public void Clear()
        {
            containers.Clear();
        }
    }
}
