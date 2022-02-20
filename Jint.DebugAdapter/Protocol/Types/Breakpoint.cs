namespace Jint.DebugAdapter.Protocol.Types
{
    internal class Breakpoint
    {
        public int? Id { get; set; }
        public bool Verified { get; set; }
        public string Message { get; set; }
        public Source Source { get; set; }
        public int? Line { get; set; }
        public int? Column { get; set; }
        public int? EndLine { get; set; }
        public int? EndColumn { get; set; }
        public string InstructionReference { get; set; }
        public int? Offset { get; set; }
    }

    internal enum EvaluationContext
    {
        Watch,
        Repl,
        Hover,
        Clipboard
    }
}
