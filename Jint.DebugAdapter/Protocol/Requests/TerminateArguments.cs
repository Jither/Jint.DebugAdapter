namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class TerminateArguments : ProtocolArguments
    {
        public bool? Restart { get; set; }
    }
}
