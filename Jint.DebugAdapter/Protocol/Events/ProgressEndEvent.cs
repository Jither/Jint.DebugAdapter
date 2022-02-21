namespace Jint.DebugAdapter.Protocol.Events
{
    public class ProgressEndEvent : ProtocolEventBody
    {
        public string ProgressId { get; set; }
        public string Message { get; set; }

        protected override string EventNameInternal => "progressEnd";
    }
}
