namespace Jint.DebugAdapter.Protocol.Events
{
    internal class TerminatedEventBody : ProtocolEventBody
    {
        public object Restart { get; set; }
    }
}
