using Jither.DebugAdapter.Protocol.Types;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public class ScopeVariableContainer : VariableContainer
    {
        private readonly DebugScope scope;
        private readonly CallFrame frame;

        public ScopeVariableContainer(VariableStore store, int id, DebugScope scope, CallFrame frame) : base(store, id)
        {
            this.scope = scope;
            this.frame = frame;
        }

        protected override IEnumerable<Variable> InternalGetVariables()
        {
            if (frame != null)
            {
                if (frame.ReturnValue != null)
                {
                    yield return CreateVariable("Return value", frame.ReturnValue);
                }
                if (!frame.This.IsUndefined())
                {
                    yield return CreateVariable("this", frame.This);
                }
            }
            foreach (var name in scope.BindingNames)
            {
                yield return CreateVariable(name, scope.GetBindingValue(name));
            }
        }
    }
}
