namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class ModulesArguments : ProtocolArguments
    {
        public int? StartModule { get; set; }
        public int? ModuleCount { get; set; }
    }
}
