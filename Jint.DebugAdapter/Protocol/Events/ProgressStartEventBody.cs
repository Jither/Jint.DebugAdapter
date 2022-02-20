namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ProgressStartEventBody : ProtocolEventBody
    {
        public string ProgressId { get; set; }
        public string Title { get; set; }
        public int? RequestId { get; set; }
        public bool? Cancellable { get; set; }
        public string Message { get; set; }
        public double? Percentage { get; set; }
    }
}
