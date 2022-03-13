using Jither.DebugAdapter.Protocol.Types;
using Jint.Native.Object;
using Jint.Runtime;
using Jint.Native;

namespace Jint.DebugAdapter.Variables
{
    public class ObjectVariableContainer : VariableContainer
    {
        private readonly ObjectInstance instance;

        public ObjectVariableContainer(VariableStore store, int id, ObjectInstance instance) : base(store, id)
        {
            this.instance = instance;
        }

        public override JsValue SetVariable(string name, JsValue value)
        {
            var prop = instance.GetOwnProperty(name);
            if (prop.Writable)
            {
                prop.Value = value;
                return value;
            }

            if (prop.Set != null)
            {
                instance.Engine.Invoke(prop.Set, value);
                return instance.Engine.Invoke(prop.Get);
            }

            throw new VariableException($"Property is read only.");
        }

        protected override IEnumerable<Variable> GetNamedVariables(int? start, int? count)
        {
            var props = instance.GetOwnProperties();

            // Return subset
            // TODO: Does this ever happen for anything except arrays in our implementation?
            if (count > 0)
            {
                props = props.Skip(start ?? 0).Take(count.Value);
            }

            return props.Select(p => CreateVariable(p.Key.ToString(), p.Value, instance));
        }

        protected override IEnumerable<Variable> GetAllVariables(int? start, int? count)
        {
            return GetNamedVariables(start, count);
        }
    }
}
