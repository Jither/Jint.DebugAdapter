namespace Jither.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event signals that the progress reporting needs to updated with a new message and/or percentage.
    /// </summary>
    /// <remarks>
    /// The client does not have to update the UI immediately, but the clients needs to keep track of the message
    /// and/or percentage values.
    /// 
    /// This event should only be sent if the client has passed the value true for the ‘supportsProgressReporting’
    /// capability of the ‘initialize’ request.
    /// </remarks>
    public class ProgressUpdateEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "progressUpdate";

        public ProgressUpdateEvent(string progressId)
        {
            ProgressId = progressId;
        }

        /// <summary>
        /// The ID that was introduced in the initial 'progressStart' event.
        /// </summary>
        public string ProgressId { get; set; }

        /// <summary>
        /// Optional, more detailed progress message. If omitted, the previous message(if any) is used.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Optional progress percentage to display (value range: 0 to 100). If omitted, no percentage will be shown.
        /// </summary>
        public double? Percentage { get; set; } // TODO: May be int 0-100 - never clarified in spec
    }
}
