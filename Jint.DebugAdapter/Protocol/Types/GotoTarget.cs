namespace Jint.DebugAdapter.Protocol.Types
{
    internal class GotoTarget
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int Line { get; set; }
        public int? Column { get; set; }
        public int? EndLine { get; set; }
        public int? EndColumn { get; set; }
        public string InstructionPointerReference { get; set; }
    }
}
