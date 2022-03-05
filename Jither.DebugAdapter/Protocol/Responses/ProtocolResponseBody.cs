using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Responses
{
    public abstract class ProtocolResponseBody
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }
    }
}
