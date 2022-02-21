using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    public class Checksum
    {
        public StringEnum<ChecksumAlgorithm> Algorithm { get; set; }
        [JsonPropertyName("checksum")]
        public string Value { get; set; }
    }
}
