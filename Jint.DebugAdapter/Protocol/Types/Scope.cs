namespace Jint.DebugAdapter.Protocol.Types
{
    internal class Scope
    {
        public string Name { get; set; }
        public string PresentationHint { get; set; }
        public int VariablesReference { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public bool Expensive { get; set; }
        public Source Source { get; set; }
        public int? Line { get; set; }
        public int? Column { get; set; }
        public int? EndLine { get; set; }
        public int? EndColumn { get; set; }
    }
}
