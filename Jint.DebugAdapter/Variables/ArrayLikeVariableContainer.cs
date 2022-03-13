using Jither.DebugAdapter.Protocol.Types;
using Jint.Native.Object;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Descriptors;
using Jint.Native.TypedArray;

namespace Jint.DebugAdapter.Variables
{
    public class ArrayLikeVariableContainer : VariableContainer
    {
        private readonly ObjectInstance instance;

        public ArrayLikeVariableContainer(VariableStore store, int id, ObjectInstance instance) : base(store, id)
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

        protected override IEnumerable<Variable> GetAllVariables(int? start, int? count)
        {
            var result = GetNamedVariables(null, 0).Concat(GetIndexedVariables(null, 0));
            // Return subset
            // TODO: Does this ever happen?
            if (count > 0)
            {
                result = result.Skip(start ?? 0).Take(count.Value);
            }
            return result;
        }

        protected override IEnumerable<Variable> GetIndexedVariables(int? start, int? count)
        {
            var items = instance is TypedArrayInstance ? GetTypedArrayIndexValues(start, count) : GetArrayIndexValues(start, count);

            return items.Select(i => CreateVariable(i.Key, i.Value));
        }

        private IEnumerable<KeyValuePair<string, JsValue>> GetArrayIndexValues(int? start, int? count)
        {
            int length = (int)instance.Length;
            if (count > 0)
            {
                length = Math.Min(length, count.Value);
            }

            // We can assume that array indices are the first Length properties returned by GetOwnProperties
            // https://tc39.es/ecma262/#sec-ordinaryownpropertykeys
            var items = instance.GetOwnProperties();
            if (start > 0)
            {
                items = items.Skip(start.Value);
            }
            return items.Take(length).Select(kv => KeyValuePair.Create(kv.Key.ToString(), kv.Value.Value));
        }

        private IEnumerable<KeyValuePair<string, JsValue>> GetTypedArrayIndexValues(int? start, int? count)
        {
            var arr = instance as TypedArrayInstance;
            
            int length = (int)arr.Length;
            if (count > 0)
            {
                length = Math.Min(length, count.Value);
            }

            var list = new List<KeyValuePair<string, JsValue>>();
            for (int i = start ?? 0; i < length; i++)
            {
                list.Add(KeyValuePair.Create(i.ToString(), arr[i]));
            }
            return list;
        }

        private IEnumerable<KeyValuePair<JsValue, PropertyDescriptor>> GetArrayProperties()
        {
            var length = instance.Length;

            // We can assume that array indices are the first Length properties returned by GetOwnProperties
            // https://tc39.es/ecma262/#sec-ordinaryownpropertykeys
            return instance.GetOwnProperties().Skip((int)length);
        }

        private IEnumerable<KeyValuePair<JsValue, PropertyDescriptor>> GetTypedArrayProperties()
        {
            // TypedArray does not include array indices in own properties
            return instance.GetOwnProperties();
        }

        protected override IEnumerable<Variable> GetNamedVariables(int? start, int? count)
        {
            var props = instance is TypedArrayInstance ? GetTypedArrayProperties() : GetArrayProperties();

            if (count > 0)
            {
                props = props.Skip(start ?? 0).Take(count.Value);
            }

            return props.Select(p => CreateVariable(p.Key.ToString(), p.Value, instance));
        }
    }
}
