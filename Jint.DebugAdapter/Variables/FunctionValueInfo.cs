using Jint.Native.Function;

namespace Jint.DebugAdapter.Variables
{
    public class FunctionValueInfo : ValueInfo
    {
        public FunctionValueInfo(string name, string valueDescription, FunctionInstance function) : base(name)
        {
            Value = valueDescription;
            Type = "Function";
        }
    }
}
