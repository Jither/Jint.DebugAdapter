using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// The checksum of an item calculated by the specified algorithm.
    /// </summary>
    public class Checksum
    {
        /// <param name="algorithm">The algorithm used to calculate this checksum.</param>
        /// <param name="value">Value of the checksum.</param>
        [JsonConstructor]
        public Checksum(ChecksumAlgorithm algorithm, string value)
        {
            Algorithm = algorithm;
            Value = value;
        }

        /// <summary>
        /// The algorithm used to calculate this checksum.
        /// </summary>
        public ChecksumAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Value of the checksum.
        /// </summary>
        [JsonPropertyName("checksum")]
        public string Value { get; set; }
    }
}
