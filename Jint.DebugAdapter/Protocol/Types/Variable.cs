namespace Jint.DebugAdapter.Protocol.Types
{
    public class Variable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public VariablePresentationHint PresentationHint { get; set; }
        public string EvaluateName { get; set; }
        public int VariablesReference { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
        public string MemoryReference { get; set; }
    }
}
