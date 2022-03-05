using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// This request retrieves the possible goto targets for the specified source location.
    /// These targets can be used in the ‘goto’ request.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsGotoTargetsRequest’ is true.
    /// </remarks>
    public class GotoTargetsArguments : ProtocolArguments
    {
        /// <summary>
        /// The source location for which the goto targets are determined.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// The line location for which the goto targets are determined.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// An optional column location for which the goto targets are determined.
        /// </summary>
        public int? Column { get; set; }
    }
}
