namespace Jint.DebugAdapter.Protocol.Requests
{
    public class TerminateArguments : ProtocolArguments
    {
        public bool? Restart { get; set; }
    }
}
