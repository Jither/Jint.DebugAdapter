namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class ReverseContinueArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public bool? SingleThread { get; set; }
    }
}
