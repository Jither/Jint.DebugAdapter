using Jint.Native.Date;
using Jint.Native.Object;
using Jint.Native.RegExp;

namespace Jint.DebugAdapter.Variables
{
    public class ObjectValueInfo : ValueInfo
    {
        public ObjectValueInfo(string name, ObjectInstance value) : base(name)
        {
            Value = value switch
            {
                DateInstance or
                RegExpInstance => value.ToString(),
                _ => "{...}" // TODO: Object preview
            };

            Type = GetObjectType(value);
        }
    }
}
