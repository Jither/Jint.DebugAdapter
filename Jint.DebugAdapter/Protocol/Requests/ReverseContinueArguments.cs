namespace Jint.DebugAdapter.Protocol.Requests
{
    public class ReverseContinueArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public bool? SingleThread { get; set; }
    }
}
