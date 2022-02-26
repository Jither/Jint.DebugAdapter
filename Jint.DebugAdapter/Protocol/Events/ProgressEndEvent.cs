namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event signals the end of the progress reporting with an optional final message.
    /// </summary>
    /// <remarks>
    /// This event should only be sent if the client has passed the value true for the
    /// ‘supportsProgressReporting’ capability of the ‘initialize’ request.
    /// </remarks>
    public class ProgressEndEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "progressEnd";

        public ProgressEndEvent(string progressId)
        {
            ProgressId = progressId;
        }

        /// <summary>
        /// The ID that was introduced in the initial 'ProgressStartEvent'.
        /// </summary>
        public string ProgressId { get; set; }

        /// <summary>
        /// Optional, more detailed progress message. If omitted, the previous message(if any) is used.
        /// </summary>
        public string Message { get; set; }
    }
}
