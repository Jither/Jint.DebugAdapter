using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘gotoTargets’ request.
    /// </summary>
    public class GotoTargetsResponse : ProtocolResponseBody
    {
        /// <param name="targets">The possible goto targets of the specified location.</param>
        public GotoTargetsResponse(IEnumerable<GotoTarget> targets)
        {
            Targets = targets;
        }

        /// <summary>
        /// The possible goto targets of the specified location.
        /// </summary>
        public IEnumerable<GotoTarget> Targets { get; set; }
    }
}
