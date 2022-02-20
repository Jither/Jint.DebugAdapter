namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class RestartFrameArguments : ProtocolArguments
    {
        public int FrameId { get; set; }
    }
}
