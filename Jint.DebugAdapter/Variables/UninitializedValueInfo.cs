namespace Jint.DebugAdapter.Variables
{
    public class UninitializedValueInfo : ValueInfo
    {
        public UninitializedValueInfo(string name) : base(name)
        {
            Value = "null";
            Type = "Not Initialized";
        }
    }
}
