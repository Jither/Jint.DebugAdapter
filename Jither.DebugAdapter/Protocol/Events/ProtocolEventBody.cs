using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Events
{
    public abstract class ProtocolEventBody
    {
        // This indirection is in order to get around JsonIgnore not being inherited if EventName was declared abstract
        [JsonIgnore]
        public string EventName => EventNameInternal;

        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }

        protected abstract string EventNameInternal { get; }
    }
}
