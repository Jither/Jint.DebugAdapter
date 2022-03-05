using Jither.DebugAdapter.Protocol.Types;
using Jint.Native.Object;

namespace Jint.DebugAdapter
{
    public class ObjectVariableContainer : VariableContainer
    {
        private readonly ObjectInstance instance;

        public ObjectVariableContainer(int id, ObjectInstance instance) : base(id)
        {
            this.instance = instance;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            throw new NotImplementedException();
        }
    }
}
