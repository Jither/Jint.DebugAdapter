namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SourceResponse : ProtocolResponseBody
    {
        public string Content { get; set; }
        public string MimeType { get; set; }
    }
}
