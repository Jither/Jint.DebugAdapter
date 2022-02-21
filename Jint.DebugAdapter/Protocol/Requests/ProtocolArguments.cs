using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal abstract class ProtocolArguments
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }
    }

}
