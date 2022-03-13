namespace Jint.DebugAdapter.Variables
{
    public class NullValueInfo : ValueInfo
    {
        public NullValueInfo(string name) : base(name)
        {
            Value = "null";
            Type = "null";
        }
    }
}
