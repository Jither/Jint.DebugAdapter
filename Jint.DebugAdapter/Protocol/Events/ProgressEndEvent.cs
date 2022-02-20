namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ProgressEndEvent : ProtocolEventBody
    {
        public string ProgressId { get; set; }
        public string Message { get; set; }

        protected override string EventNameInternal => "progressEnd";
    }
}
