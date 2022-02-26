namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// This event signals that some state in the debug adapter has changed and requires that the client needs to
    /// re-render the data snapshot previously requested.
    /// </summary>
    /// <remarks>
    /// Debug adapters do not have to emit this event for runtime changes like stopped or thread events because in
    /// that case the client refetches the new state anyway. But the event can be used for example to refresh the UI
    /// after rendering formatting has changed in the debug adapter.
    /// 
    /// This event should only be sent if the debug adapter has received a value true for the
    /// ‘supportsInvalidatedEvent’ capability of the ‘initialize’ request.
    /// </remarks>
    public class InvalidatedEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "invalidated";

        /// <summary>
        /// Optional set of logical areas that got invalidated.
        /// </summary>
        /// <remarks>
        /// This property has a hint characteristic: a client can only be expected to make a 'best effort' in
        /// honouring the areas but there are no guarantees. If this property is missing, empty, or if values are not
        /// understood the client should assume a single value 'all'.
        /// </remarks>
        // export type InvalidatedAreas = 'all' | 'stacks' | 'threads' | 'variables' | string
        public List<string> InvalidatedAreas { get; set; }

        /// <summary>
        /// If specified, the client only needs to refetch data related to this thread.
        /// </summary>
        public int? ThreadId { get; set; }

        /// <summary>
        /// If specified, the client only needs to refetch data related to this stack frame
        /// (and the 'threadId' is ignored).
        /// </summary>
        public int? StackFrameId { get; set; }
    }
}
