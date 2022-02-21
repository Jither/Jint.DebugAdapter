namespace Jint.DebugAdapter.Protocol.Events
{
    public class ExitedEvent : ProtocolEventBody
    {
        public int ExitCode { get; set; }

        protected override string EventNameInternal => "exited";
    }
}
