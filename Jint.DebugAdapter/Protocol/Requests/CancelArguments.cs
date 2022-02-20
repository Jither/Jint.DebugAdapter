namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class CancelArguments : ProtocolArguments
    {
        public uint RequestId { get; set; }
        public string ProgressId { get; set; }
    }
}
