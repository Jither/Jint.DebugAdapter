namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘source’ request.
    /// </summary>
    public class SourceResponse : ProtocolResponseBody
    {
        /// <param name="content">Content of the source reference.</param>
        public SourceResponse(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Content of the source reference.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Optional content type (mime type) of the source.
        /// </summary>
        public string MimeType { get; set; }
    }
}
