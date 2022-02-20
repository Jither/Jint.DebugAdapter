using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal abstract class ProtocolResponseBody
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }
    }
}
