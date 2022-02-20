namespace Jint.DebugAdapter.Protocol.Events
{
    internal class TerminatedEvent : ProtocolEventBody
    {
        public object Restart { get; set; }

        protected override string EventNameInternal => "terminated";
    }
}
