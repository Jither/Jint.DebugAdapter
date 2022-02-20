namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class DataBreakpointInfoArguments : ProtocolArguments
    {
        public int? VariablesReference { get; set; }
        public string Name { get; set; }
    }
}
