using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public abstract class VariableContainer
    {
        public int Id { get; }
        
        protected VariableContainer(int id)
        {
            Id = id;
        }

        public IEnumerable<Variable> GetVariables()
        {
            return InternalGetVariables();
        }

        protected abstract IEnumerable<Variable> InternalGetVariables();
    }
}
