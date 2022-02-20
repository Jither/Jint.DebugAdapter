namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class PauseArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
    }
}
