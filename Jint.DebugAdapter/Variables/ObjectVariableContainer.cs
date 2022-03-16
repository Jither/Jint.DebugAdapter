using Jither.DebugAdapter.Protocol.Types;
using Jint.Native.Object;
using Jint.Runtime;
using Jint.Native;
using Jint.Runtime.Descriptors;

namespace Jint.DebugAdapter.Variables
{
    public class ObjectVariableContainer : VariableContainer
    {
        protected readonly ObjectInstance instance;

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

        protected override IEnumerable<JintVariable> GetNamedVariables(int? start, int? count)
        {
            var props = instance.GetOwnProperties();
            props = props.Concat(GetPrototypeProperties());

            // Return subset
            // TODO: Does this ever happen for anything except arrays in our implementation?
            if (count > 0)
            {
                props = props.Skip(start ?? 0).Take(count.Value);
            }

            return AddPrototypeIfExists(props.Select(p => CreateVariable(p.Key.ToString(), p.Value, instance)));
        }

        protected override IEnumerable<JintVariable> GetAllVariables(int? start, int? count)
        {
            return GetNamedVariables(start, count);
        }

        protected IEnumerable<JintVariable> AddPrototypeIfExists(IEnumerable<JintVariable> vars)
        {
            if (instance.Prototype != null)
            {
                var prototype = CreateVariable("[[Prototype]]", instance.Prototype);
                // For prototypes, we want the value to display the prototype's constructor ("type") (a la Chromium devtools)
                prototype.Value = prototype.Type;
                // Place last
                prototype.SortOrder = 10000;
                vars = vars.Append(prototype);
            }

            return vars;
        }

        protected IEnumerable<KeyValuePair<JsValue, PropertyDescriptor>> GetPrototypeProperties()
        {
            // TODO: Handle shadowed prototype properties
            var proto = instance.Prototype;
            while (proto != null && proto is not ObjectConstructor)
            {
                var props = proto.GetOwnProperties();
                foreach (var prop in props)
                {
                    if (prop.Value.Get != null)
                    {
                        yield return prop;
                    }
                }
                proto = proto.Prototype;
            }
        }
    }
}
