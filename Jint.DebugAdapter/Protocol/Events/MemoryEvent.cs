namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// This event indicates that some memory range has been updated.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     It should only be sent if the debug adapter has received a value true for the 
    ///     supportsMemoryEvent capability of the initialize request.
    ///     </para>
    ///     <para>
    ///     Clients typically react to the event by re-issuing a readMemory request if they show
    ///     the memory identified by the memoryReference and if the updated memory range overlaps
    ///     the displayed range.Clients should not make assumptions how individual memory references
    ///     relate to each other, so they should not assume that they are part of a single continuous
    ///     address range and might overlap.
    ///     </para>
    ///     <para>
    ///     Debug adapters can use this event to indicate that the contents of a memory range has 
    ///     changed due to some other DAP request like setVariable or setExpression.Debug adapters are
    ///     not expected to emit this event for each and every memory change of a running program,
    ///     because that information is typically not available from debuggers and it would flood
    ///     clients with too many events.
    ///     </para>
    /// </remarks>
    public class MemoryEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "memory";

        public MemoryEvent(string memoryReference, long offset, long count)
        {
            MemoryReference = memoryReference;
            Offset = offset;
            Count = count;
        }

        /// <summary>
        /// Memory reference of a memory range that has been updated.
        /// </summary>
        public string MemoryReference { get; set; }

        /// <summary>
        /// Starting offset in bytes where memory has been updated. Can be negative.
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Number of bytes updated.
        /// </summary>
        public long Count { get; set; }
    }
}
