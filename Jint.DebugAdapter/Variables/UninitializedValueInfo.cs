namespace Jint.DebugAdapter.Variables
{
    public class UninitializedValueInfo : ValueInfo
    {
        public UninitializedValueInfo(string name, string value) : base(name)
        {
            Value = value;
            Type = "Not Initialized";
        }
    }
}
