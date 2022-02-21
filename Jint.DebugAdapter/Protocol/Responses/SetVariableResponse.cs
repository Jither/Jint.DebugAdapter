namespace Jint.DebugAdapter.Protocol.Responses
{
    public class SetVariableResponse : ProtocolResponseBody
    {
        public string Value { get; set; }
        public string Type { get; set; }
        public int? VariablesReference { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
    }
}
