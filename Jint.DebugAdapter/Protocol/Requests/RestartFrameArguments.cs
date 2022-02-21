namespace Jint.DebugAdapter.Protocol.Requests
{
    public class RestartFrameArguments : ProtocolArguments
    {
        public int FrameId { get; set; }
    }
}
