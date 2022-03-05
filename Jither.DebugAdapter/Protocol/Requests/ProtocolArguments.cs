using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Requests
{
    public abstract class ProtocolArguments
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }
    }

}
