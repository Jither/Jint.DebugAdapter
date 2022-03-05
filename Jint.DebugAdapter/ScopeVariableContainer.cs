using Jither.DebugAdapter.Protocol.Types;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public class ScopeVariableContainer : VariableContainer
    {
        private readonly DebugScope scope;

        public ScopeVariableContainer(VariableStore store, int id, DebugScope scope) : base(store, id)
        {
            this.scope = scope;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            return scope.BindingNames.Select(n => CreateVariable(n, scope.GetBindingValue(n)));
        }
    }
}
