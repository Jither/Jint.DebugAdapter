using System.Text.Json;
using Jint.Native;

namespace Jint.DebugAdapter.Variables
{
    public class PrimitiveValueInfo : ValueInfo
    {
        public PrimitiveValueInfo(string name, string value, string type) : base(name)
        {
            Value = value;
            Type = type;
        }
    }
}
