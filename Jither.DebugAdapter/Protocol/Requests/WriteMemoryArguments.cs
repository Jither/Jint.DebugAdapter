namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Writes bytes to memory at the provided location.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsWriteMemoryRequest’ is true.
    /// </remarks>
    public class WriteMemoryArguments : ProtocolArguments
    {
        /// <summary>
        /// Memory reference to the base location to which data should be written.
        /// </summary>
        public string MemoryReference { get; set; }

        /// <summary>
        /// Optional offset (in bytes) to be applied to the reference location before writing data. Can be negative.
        /// </summary>
        public long? Offset { get; set; }

        /// <summary>
        /// Optional property to control partial writes.
        /// </summary>
        /// <remarks>
        /// If true, the debug adapter should attempt to write memory even if the entire memory region is not 
        /// writable. In such a case the debug adapter should stop after hitting the first byte of memory that cannot
        /// be written and return the number of bytes written in the response via the 'offset' and 'bytesWritten'
        /// properties. If false or missing, a debug adapter should attempt to verify the region is writable before
        /// writing, and fail the response if it is not.
        /// </remarks>
        public bool? AllowPartial { get; set; }

        /// <summary>
        /// Bytes to write, encoded using base64.
        /// </summary>
        public string Data { get; set; }
    }
}
