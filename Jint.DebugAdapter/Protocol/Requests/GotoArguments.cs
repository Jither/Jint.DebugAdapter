namespace Jint.DebugAdapter.Protocol.Requests
{
    public class GotoArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public int TargetId { get; set; }
    }
}
