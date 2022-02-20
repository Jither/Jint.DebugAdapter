namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ProgressUpdateEvent : ProtocolEventBody
    {
        public string ProgressId { get; set; }
        public string Message { get; set; }
        public double? Percentage { get; set; }

        protected override string EventNameInternal => "progressUpdate";
    }
}
