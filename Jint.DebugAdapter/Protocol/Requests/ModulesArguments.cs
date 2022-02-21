namespace Jint.DebugAdapter.Protocol.Requests
{
    public class ModulesArguments : ProtocolArguments
    {
        public int? StartModule { get; set; }
        public int? ModuleCount { get; set; }
    }
}
