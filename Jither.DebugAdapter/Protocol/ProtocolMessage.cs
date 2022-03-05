using System.Text.Json;
using System.Text.Json.Serialization;
using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol
{
    [JsonConverter(typeof(ProtocolMessageConverter))]
    public abstract class ProtocolMessage
    {
        [JsonPropertyOrder(-100)]
        public string Type { get; set; }

        [JsonPropertyOrder(-1)]
        public int Seq { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }
    }
}
