namespace Jint.DebugAdapter.Protocol.Events
{
    public class TerminatedEvent : ProtocolEventBody
    {
        public object Restart { get; set; }

        protected override string EventNameInternal => "terminated";
    }
}
