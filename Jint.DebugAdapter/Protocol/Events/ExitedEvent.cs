namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ExitedEvent : ProtocolEventBody
    {
        public int ExitCode { get; set; }

        protected override string EventNameInternal => "exited";
    }
}
