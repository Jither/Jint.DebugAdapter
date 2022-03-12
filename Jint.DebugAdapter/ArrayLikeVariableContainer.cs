using Jither.DebugAdapter.Protocol.Types;
using Jint.Native.Object;
using Jint.Native;
using Jint.Runtime;

namespace Jint.DebugAdapter
{
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

            // We can assume that array indices are the first Length properties returned by GetOwnProperties
            // https://tc39.es/ecma262/#sec-ordinaryownpropertykeys
            var items = instance.GetOwnProperties().Take((int)length);

            if (count > 0)
            {
                items = items.Skip(start ?? 0).Take(count.Value);
            }

            return items.Select(i => CreateVariable(i.Key.ToString(), i.Value, instance));
        }

        protected override IEnumerable<Variable> GetNamedVariables(int? start, int? count)
        {
            var length = instance.Length;

            // We can assume that array indices are the first Length properties returned by GetOwnProperties
            // https://tc39.es/ecma262/#sec-ordinaryownpropertykeys
            var props = instance.GetOwnProperties().Skip((int)length);

            if (count > 0)
            {
                props = props.Skip(start ?? 0).Take(count.Value);
            }

            return props.Select(p => CreateVariable(p.Key.ToString(), p.Value, instance));
        }
    }
}
