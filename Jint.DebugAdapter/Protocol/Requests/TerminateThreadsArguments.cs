namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class TerminateThreadsArguments : ProtocolArguments
    {
        public List<int> ThreadIds { get; set; }
    }
}
