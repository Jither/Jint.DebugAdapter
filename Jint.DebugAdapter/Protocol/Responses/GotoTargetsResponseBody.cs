using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class GotoTargetsResponseBody : ProtocolResponseBody
    {
        public List<GotoTarget> Targets { get; set; }
    }
}
