using Jint.Native.Object;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public class VariableStore
    {
        private int nextId = 1;
        private readonly Dictionary<int, VariableContainer> containers = new();

        public int Add(DebugScope scope)
        {
            var container = new ScopeVariableContainer(nextId++, scope);
            containers.Add(container.Id, container);
            return container.Id;
        }

        public int Add(ObjectInstance instance)
        {
            var container = new ObjectVariableContainer(nextId++, instance);
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
