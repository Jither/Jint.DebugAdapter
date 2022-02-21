namespace Jint.DebugAdapter.Protocol.Responses
{
    public class SourceResponse : ProtocolResponseBody
    {
        public string Content { get; set; }
        public string MimeType { get; set; }
    }
}
