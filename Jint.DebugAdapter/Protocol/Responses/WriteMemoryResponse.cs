namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Writes bytes to memory at the provided location.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsWriteMemoryRequest’ is true.
    /// </remarks>
    public class WriteMemoryResponse : ProtocolResponseBody
    {
        public WriteMemoryResponse()
        {

        }

        /// <summary>
        /// Optional property that should be returned when 'allowPartial' is true to indicate the offset of the
        /// first byte of data successfully written.Can be negative.
        /// </summary>
        public long? Offset { get; set; }

        /// <summary>
        /// Optional property that should be returned when 'allowPartial' is true to indicate the number of bytes
        /// starting from address that were successfully written.
        /// </summary>
        public int? BytesWritten { get; set; }
    }
}
