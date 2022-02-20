using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal abstract class ProtocolEventBody
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }
    }
}
