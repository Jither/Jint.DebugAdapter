using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal enum ChecksumAlgorithm
    {
        Other,
        [JsonPropertyName("MD5")]
        MD5,
        [JsonPropertyName("SHA1")]
        SHA1,
        [JsonPropertyName("SHA256")]
        SHA256,
        Timestamp
    }
}
