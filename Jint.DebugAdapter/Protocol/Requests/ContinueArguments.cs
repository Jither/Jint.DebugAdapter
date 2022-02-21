namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class ContinueArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public bool? SingleThread { get; set; }
    }
}
