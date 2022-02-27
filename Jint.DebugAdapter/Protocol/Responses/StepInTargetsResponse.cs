using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘stepInTargets’ request.
    /// </summary>
    public class StepInTargetsResponse : ProtocolResponseBody
    {
        /// <param name="targets">The possible stepIn targets of the specified source location.</param>
        public StepInTargetsResponse(List<StepInTarget> targets)
        {
            Targets = targets;
        }

        /// <summary>
        /// The possible stepIn targets of the specified source location.
        /// </summary>
        public List<StepInTarget> Targets { get; set; }
    }
}
