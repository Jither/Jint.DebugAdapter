using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Represents a single disassembled instruction.
    /// </summary>
    public class DisassembledInstruction
    {
        /// <param name="address">The address of the instruction.</param>
        /// <param name="instruction">Text representing the instruction and its operands, in an
        /// implementation-defined format.</param>
        [JsonConstructor]
        public DisassembledInstruction(string address, string instruction)
        {
            Address = address;
            Instruction = instruction;
        }

        /// <summary>
        /// The address of the instruction. Treated as a hex value if prefixed with '0x',
        /// or as a decimal value otherwise.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Optional raw bytes representing the instruction and its operands, in an implementation-defined format.
        /// </summary>
        public string InstructionBytes { get; set; }

        /// <summary>
        /// Text representing the instruction and its operands, in an implementation-defined format.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// Name of the symbol that corresponds with the location of this instruction, if any.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Source location that corresponds to this instruction, if any.
        /// </summary>
        /// <remarks>
        /// Should always be set(if available) on the first instruction returned, but can be omitted afterwards if
        /// this instruction maps to the same source file as the previous instruction.
        /// </remarks>
        public Source Location { get; set; }

        /// <summary>
        /// The line within the source location that corresponds to this instruction, if any.
        /// </summary>
        public int? Line { get; set; }

        /// <summary>
        /// The column within the line that corresponds to this instruction, if any.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// The end line of the range that corresponds to this instruction, if any.
        /// </summary>
        public int? EndLine { get; set; }

        /// <summary>
        /// The end column of the range that corresponds to this instruction, if any.
        /// </summary>
        public int? EndColumn { get; set; }
    }
}
