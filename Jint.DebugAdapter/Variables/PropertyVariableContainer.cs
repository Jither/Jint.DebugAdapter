using Jither.DebugAdapter.Protocol.Types;
using Jint.Runtime.Descriptors;
using Jint.Native.Object;
using Jint.Native;

namespace Jint.DebugAdapter.Variables
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

        public override JsValue SetVariable(string name, JsValue value)
        {
            // TODO: Is this right?
            throw new VariableException($"Cannot modify property value {name}");
        }

        protected override IEnumerable<Variable> GetAllVariables(int? start, int? count)
        {
            return new[] { CreateVariable(string.Empty, engine.Invoke(prop.Get, owner, Array.Empty<object>())) };
        }
    }
}
