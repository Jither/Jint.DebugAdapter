namespace Jint.DebugAdapter.Protocol.Types
{
    public class VariablePresentationHint
    {
        public string Kind { get; set; }
        public List<string> Attributes { get; set; }
        public string Visibility { get; set; }
    }
}
