using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class DisassembleResponse : ProtocolResponseBody
    {
        public List<DisassembledInstruction> Instructions { get; set; }
    }
}
