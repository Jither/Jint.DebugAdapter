using Jither.DebugAdapter.Protocol.Types;
using Jint.Native.Object;

namespace Jint.DebugAdapter
{
    public class ObjectVariableContainer : VariableContainer
    {
        private readonly ObjectInstance instance;

        public ObjectVariableContainer(VariableStore store, int id, ObjectInstance instance) : base(store, id)
        {
            this.instance = instance;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            return instance.GetOwnProperties().Select(p => CreateVariable(p.Key.ToString(), p.Value, instance));
        }
    }
}
