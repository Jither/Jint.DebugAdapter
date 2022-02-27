using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘gotoTargets’ request.
    /// </summary>
    public class GotoTargetsResponse : ProtocolResponseBody
    {
        /// <param name="targets">The possible goto targets of the specified location.</param>
        public GotoTargetsResponse(List<GotoTarget> targets)
        {
            Targets = targets;
        }

        /// <summary>
        /// The possible goto targets of the specified location.
        /// </summary>
        public List<GotoTarget> Targets { get; set; }
    }
}
