namespace Jint.DebugAdapter.Protocol.Requests
{
    public class PauseArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
    }
}
