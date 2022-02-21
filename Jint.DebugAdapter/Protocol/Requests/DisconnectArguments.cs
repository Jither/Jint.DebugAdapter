namespace Jint.DebugAdapter.Protocol.Requests
{
    public class DisconnectArguments : ProtocolArguments
    {
        public bool? Restart { get; set; }
        public bool? TerminateDebuggee { get; set; }
        public bool? SuspendDebuggee { get; set; }
    }
}
