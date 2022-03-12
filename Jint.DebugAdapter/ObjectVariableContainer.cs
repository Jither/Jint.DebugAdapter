using Jither.DebugAdapter.Protocol.Types;
using Jint.Native.Object;
using Jint.Native;
using Jint.Runtime;

namespace Jint.DebugAdapter
{
    public class ObjectVariableContainer : VariableContainer
    {
        private readonly ObjectInstance instance;

        public ObjectVariableContainer(VariableStore store, int id, ObjectInstance instance) : base(store, id)
        {
            this.instance = instance;
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

    public class ArrayLikeVariableContainer : VariableContainer
    {
        private readonly ObjectInstance instance;

        public ArrayLikeVariableContainer(VariableStore store, int id, ObjectInstance instance) : base(store, id)
        {
            this.instance = instance;
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
            var length = instance.Length;
            // TODO: Can we assume that GetOwnProperties always returns array indices first?
            var items = instance.GetOwnProperties().Where(p => IsArrayIndex(p.Key));

            if (count > 0)
            {
                items = items.Skip(start ?? 0).Take(count.Value);
            }

            return items.Select(i => CreateVariable(i.Key.ToString(), i.Value, instance));
        }

        protected override IEnumerable<Variable> GetNamedVariables(int? start, int? count)
        {
            var props = instance.GetOwnProperties().Where(p => !IsArrayIndex(p.Key));

            if (count > 0)
            {
                props = props.Skip(start ?? 0).Take(count.Value);
            }

            return props.Select(p => CreateVariable(p.Key.ToString(), p.Value, instance));
        }

        private static bool IsArrayIndex(JsValue key)
        {
            uint index = TypeConverter.ToUint32(key);
            return TypeConverter.ToString(key) == TypeConverter.ToString(index) && index < UInt32.MaxValue; 
        }
    }
}
