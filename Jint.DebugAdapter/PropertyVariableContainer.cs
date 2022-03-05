using Jither.DebugAdapter.Protocol.Types;
using Jint.Runtime.Descriptors;
using Jint.Native.Object;

namespace Jint.DebugAdapter
{
    public class PropertyVariableContainer : VariableContainer
    {
        private readonly PropertyDescriptor prop;
        private readonly Engine engine;
        private readonly ObjectInstance owner;

        public PropertyVariableContainer(VariableStore store, int id, PropertyDescriptor prop, ObjectInstance owner, Engine engine) : base(store, id)
        {
            this.prop = prop;
            this.owner = owner;
            this.engine = engine;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            return new[] { CreateVariable(String.Empty, engine.Invoke(prop.Get, owner, Array.Empty<object>())) };
        }
    }
}
