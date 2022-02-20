namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ExitedEventBody : ProtocolEventBody
    {
        public int ExitCode { get; set; }
    }
}
