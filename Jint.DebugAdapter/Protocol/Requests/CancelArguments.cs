namespace Jint.DebugAdapter.Protocol.Requests
{
    public class CancelArguments : ProtocolArguments
    {
        public uint RequestId { get; set; }
        public string ProgressId { get; set; }
    }
}
