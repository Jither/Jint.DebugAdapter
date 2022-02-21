namespace Jint.DebugAdapter.Protocol.Events
{
    public class ContinuedEvent : ProtocolEventBody
    {
        public int ThreadId { get; set; }
        public bool? AllThreadsContinued { get; set; }

        protected override string EventNameInternal => "continued";
    }
}
