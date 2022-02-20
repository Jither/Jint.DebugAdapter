using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class Checksum
    {
        public string Algorithm { get; set; }
        [JsonPropertyName("checksum")]
        public string Value { get; set; }
    }
}
