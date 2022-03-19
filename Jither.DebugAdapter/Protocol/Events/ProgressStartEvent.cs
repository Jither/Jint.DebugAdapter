namespace Jither.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event signals that a long running operation is about to start and provides additional information
    /// for the client to set up a corresponding progress and cancellation UI.
    /// </summary>
    /// <remarks>
    /// The client is free to delay the showing of the UI in order to reduce flicker.
    /// This event should only be sent if the client has passed the value true for the
    /// ‘supportsProgressReporting’ capability of the ‘initialize’ request.
    /// </remarks>
    public class ProgressStartEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "progressStart";

        public ProgressStartEvent(string progressId, string title)
        {
            ProgressId = progressId;
            Title = title;
        }

        /// <summary>
        /// An ID that must be used in subsequent 'progressUpdate' and 'progressEnd' events to make them refer to
        /// the same progress reporting.
        /// </summary>
        /// <remarks>
        /// IDs must be unique within a debug session.
        /// </remarks>
        public string ProgressId { get; set; }

        /// <summary>
        /// Mandatory (short) title of the progress reporting.
        /// </summary>
        /// <remarks>
        /// Shown in the UI to describe the long running operation.
        /// </remarks>
        public string Title { get; set; }

        /// <summary>
        /// The request ID that this progress report is related to.
        /// </summary>
        /// <remarks>
        /// If specified, a debug adapter is expected to emit progress events for the long running request
        /// until the request has been either completed or cancelled. If the request ID is omitted, the 
        /// progress report is assumed to be related to some general activity of the debug adapter.
        /// </remarks>
        public int? RequestId { get; set; }

        /// <summary>
        /// If true, the request that reports progress may be canceled with a 'cancel' request.
        /// </summary>
        /// <remarks>
        /// So this property basically controls whether the client should use UX that supports cancellation.
        /// Clients that don't support cancellation are allowed to ignore the setting.
        /// </remarks>
        public bool? Cancellable { get; set; }

        /// <summary>
        /// Optional, more detailed progress message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Optional progress percentage to display (value range: 0 to 100). If omitted no percentage will be shown.
        /// </summary>
        /// <remarks>
        /// Note that the DebugAdapter specification currently does not specify whether this is intended as
        /// integer values 0 to 100, or floating point.
        /// </remarks>
        public double? Percentage { get; set; }
    }
}
