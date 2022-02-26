namespace Jint.DebugAdapter.Protocol.Requests
{
    public class CancelArguments : ProtocolArguments
    {
        /// <summary>
        /// The ID (attribute 'seq') of the request to cancel. If missing, no request is cancelled.
        /// </summary>
        /// <remarks>
        /// Both a 'requestId' and a 'progressId' can be specified in one request.
        /// </remarks>
        public uint RequestId { get; set; }

        /// <summary>
        /// The ID (attribute 'progressId') of the progress to cancel. If missing, no progress is cancelled.
        /// </summary>
        /// <remarks>
        /// Both a 'requestId' and a 'progressId' can be specified in one request.
        /// </remarks>
        public string ProgressId { get; set; }
    }
}
