namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class GotoArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public int TargetId { get; set; }
    }
}
