using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘stepInTargets’ request.
    /// </summary>
    public class StepInTargetsResponse : ProtocolResponseBody
    {
        /// <param name="targets">The possible stepIn targets of the specified source location.</param>
        public StepInTargetsResponse(IEnumerable<StepInTarget> targets)
        {
            Targets = targets;
        }

        /// <summary>
        /// The possible stepIn targets of the specified source location.
        /// </summary>
        public IEnumerable<StepInTarget> Targets { get; set; }
    }
}
