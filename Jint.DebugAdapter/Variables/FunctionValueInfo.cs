using Jint.Native.Function;

namespace Jint.DebugAdapter.Variables
{
    public class FunctionValueInfo : ValueInfo
    {
        public FunctionValueInfo(string name, FunctionInstance function) : base(name)
        {
            Value = $"ƒ {name}";
            Type = "Function";
        }
    }
}
