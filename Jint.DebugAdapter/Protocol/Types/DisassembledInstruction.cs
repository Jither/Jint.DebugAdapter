namespace Jint.DebugAdapter.Protocol.Types
{
    public class DisassembledInstruction
    {
        public string Address { get; set; }
        public string InstructionBytes { get; set; }
        public string Instruction { get; set; }
        public string Symbol { get; set; }
        public Source Location { get; set; }
        public int? Line { get; set; }
        public int? Column { get; set; }
        public int? EndLine { get; set; }
        public int? EndColumn { get; set; }
    }
}
