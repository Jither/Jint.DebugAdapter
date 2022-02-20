namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ThreadsResponseBody : ProtocolResponseBody
    {
        public List<Types.Thread> Threads { get; set; }
    }
}
