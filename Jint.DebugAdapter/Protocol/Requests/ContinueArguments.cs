namespace Jint.DebugAdapter.Protocol.Requests
{
    public class ContinueArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public bool? SingleThread { get; set; }
    }
}
