using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class DisassembleResponseBody : ProtocolResponseBody
    {
        public List<DisassembledInstruction> Instructions { get; set; }
    }
}
