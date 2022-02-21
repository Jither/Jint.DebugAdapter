namespace Jint.DebugAdapter.Protocol.Requests
{
    public class DataBreakpointInfoArguments : ProtocolArguments
    {
        public int? VariablesReference { get; set; }
        public string Name { get; set; }
    }
}
