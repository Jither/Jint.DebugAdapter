namespace Jint.DebugAdapter.Protocol.Requests
{
    public class TerminateThreadsArguments : ProtocolArguments
    {
        public List<int> ThreadIds { get; set; }
    }
}
