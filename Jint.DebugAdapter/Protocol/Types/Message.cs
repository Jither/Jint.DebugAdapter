namespace Jint.DebugAdapter.Protocol.Types
{
    public class Message
    {
        public int Id { get; set; }
        public string Format { get; set; }
        public Dictionary<string, string> Variables { get; set; }
        public bool? SendTelemetry { get; set; }
        public bool? ShowUser { get; set; }
        public string Url { get; set; }
        public string UrlLabel { get; set; }
    }
}
