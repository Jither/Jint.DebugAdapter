using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request retrieves the source code for a given source reference.
    /// Either source.path or source.sourceReference must be specified.
    /// </summary>
    public class SourceArguments : ProtocolArguments
    {
        /// <summary>
        /// Specifies the source content to load.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// The reference to the source. This is the same as source.sourceReference.
        /// </summary>
        [Obsolete("This is provided for backward compatibility since old backends do not understand the 'source' attribute.")]
        public int SourceReference { get; set; }
    }
}
