using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class Checksum
    {
        public ChecksumAlgorithm Algorithm { get; set; }
        [JsonPropertyName("checksum")]
        public string Value { get; set; }
    }
}
