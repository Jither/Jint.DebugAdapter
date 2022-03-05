using Jither.DebugAdapter.Protocol.Types;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public class ScopeVariableContainer : VariableContainer
    {
        private readonly DebugScope scope;

        public ScopeVariableContainer(int id, DebugScope scope) : base(id)
        {
            this.scope = scope;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            return scope.BindingNames.Select(n => new Variable(n, scope.GetBindingValue(n).ToString()));
        }
    }
}
