using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class StepInTargetsResponse : ProtocolResponseBody
    {
        public List<StepInTarget> Targets { get; set; }
    }
}
