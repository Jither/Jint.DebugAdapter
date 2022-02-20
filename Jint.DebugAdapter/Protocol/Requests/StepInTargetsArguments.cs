namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class StepInTargetsArguments : ProtocolArguments
    {
        public int FrameId { get; set; }
    }
}
