using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘disassemble’ request.
    /// </summary>
    public class DisassembleResponse : ProtocolResponseBody
    {
        /// <param name="instructions">The list of disassembled instructions.</param>
        public DisassembleResponse(List<DisassembledInstruction> instructions)
        {
            Instructions = instructions;
        }

        /// <summary>
        /// The list of disassembled instructions.
        /// </summary>
        public List<DisassembledInstruction> Instructions { get; set; }
    }
}
