using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class DisassembleResponse : ProtocolResponseBody
    {
        public List<DisassembledInstruction> Instructions { get; set; }
    }
}
