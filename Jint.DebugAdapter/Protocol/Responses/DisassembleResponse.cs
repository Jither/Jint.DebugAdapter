using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘disassemble’ request.
    /// </summary>
    public class DisassembleResponse : ProtocolResponseBody
    {
        /// <param name="instructions">The list of disassembled instructions.</param>
        public DisassembleResponse(IEnumerable<DisassembledInstruction> instructions)
        {
            Instructions = instructions;
        }

        /// <summary>
        /// The list of disassembled instructions.
        /// </summary>
        public IEnumerable<DisassembledInstruction> Instructions { get; set; }
    }
}
