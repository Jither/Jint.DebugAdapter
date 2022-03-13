using Jint.Native.Date;
using Jint.Native.Object;
using Jint.Native.RegExp;

namespace Jint.DebugAdapter.Variables
{
    public class ObjectValueInfo : ValueInfo
    {
        public ObjectValueInfo(string name, string valueDescription, ObjectInstance value) : base(name)
        {
            Value = valueDescription;
            Type = GetObjectType(value);
        }
    }
}
