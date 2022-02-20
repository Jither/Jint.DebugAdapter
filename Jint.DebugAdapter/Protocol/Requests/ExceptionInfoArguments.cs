namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class ExceptionInfoArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
    }
}
