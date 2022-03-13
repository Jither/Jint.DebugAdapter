using System.Text.Json;
using Jint.Native;

namespace Jint.DebugAdapter.Variables
{
    public class PrimitiveValueInfo : ValueInfo
    {
        public PrimitiveValueInfo(string name, JsValue value) : base(name)
        {
            Value = value switch
            {
                JsString => JsonSerializer.Serialize(value.ToString(), stringToJsonOptions),
                _ => value.ToString()
            };
            Type = value.Type.ToString();
        }
    }
}
