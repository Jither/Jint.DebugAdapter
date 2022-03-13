using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Variables
{
    public class GetterValueInfo : ValueInfo
    {
        public GetterValueInfo(string name) : base(name)
        {
            Value = "(...)";
            PresentationHint = new VariablePresentationHint { Lazy = true };
        }
    }
}
