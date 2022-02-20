using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class StepInTargetsResponse : ProtocolResponseBody
    {
        public List<StepInTarget> Targets { get; set; }
    }
}
