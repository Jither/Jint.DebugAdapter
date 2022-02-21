using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class GotoTargetsResponse : ProtocolResponseBody
    {
        public List<GotoTarget> Targets { get; set; }
    }
}
