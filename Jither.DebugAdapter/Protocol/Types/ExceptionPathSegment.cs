using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// An ExceptionPathSegment represents a segment in a path that is used to match leafs or nodes
    /// in a tree of exceptions.
    /// </summary>
    /// <remarks>
    /// If a segment consists of more than one name, it matches the names provided if ‘negate’ is false or missing or
    /// it matches anything except the names provided if ‘negate’ is true.
    /// </remarks>
    public class ExceptionPathSegment
    {
        /// <param name="names">Depending on the value of 'negate' the names that should match or not match.</param>
        [JsonConstructor]
        public ExceptionPathSegment(List<string> names)
        {
            Names = names;
        }

        /// <summary>
        /// If false or missing this segment matches the names provided, otherwise it matches anything except the
        /// names provided.
        /// </summary>
        public bool? Negate { get; set; }

        /// <summary>
        /// Depending on the value of 'negate' the names that should match or not match.
        /// </summary>
        public List<string> Names { get; set; }
    }
}
