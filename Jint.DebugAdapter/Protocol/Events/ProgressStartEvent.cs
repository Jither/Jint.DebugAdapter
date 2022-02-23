namespace Jint.DebugAdapter.Protocol.Events
{
    public class ProgressStartEvent : ProtocolEventBody
    {
        public string ProgressId { get; set; }
        public string Title { get; set; }
        public int? RequestId { get; set; }
        public bool? Cancellable { get; set; }
        public string Message { get; set; }
        public double? Percentage { get; set; } // TODO: May be int 0-100 - never clarified in spec

        protected override string EventNameInternal => "progressStart";
    }
}
