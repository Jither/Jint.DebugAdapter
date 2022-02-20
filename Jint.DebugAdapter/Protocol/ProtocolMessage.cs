using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol
{
    [JsonConverter(typeof(ProtocolMessageConverter))]
    internal abstract class ProtocolMessage
    {
        [JsonPropertyOrder(-100)]
        public string Type { get; set; }

        [JsonPropertyOrder(-1)]
        public int Seq { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; }
    }
}
