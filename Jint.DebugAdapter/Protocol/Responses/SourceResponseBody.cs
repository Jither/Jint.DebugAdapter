namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SourceResponseBody : ProtocolResponseBody
    {
        public string Content { get; set; }
        public string MimeType { get; set; }
    }
}
