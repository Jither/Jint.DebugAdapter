namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ProgressUpdateEventBody : ProtocolEventBody
    {
        public string ProgressId { get; set; }
        public string Message { get; set; }
        public double? Percentage { get; set; }
    }
}
