namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ThreadsResponse : ProtocolResponseBody
    {
        public List<Types.Thread> Threads { get; set; }
    }
}
