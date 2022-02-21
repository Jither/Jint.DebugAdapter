namespace Jint.DebugAdapter.Protocol.Responses
{
    public class ThreadsResponse : ProtocolResponseBody
    {
        public List<Types.Thread> Threads { get; set; }
    }
}
