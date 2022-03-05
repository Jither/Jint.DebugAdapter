namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Reads bytes from memory at the provided location.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsReadMemoryRequest’ is true.
    /// </remarks>
    public class ReadMemoryArguments : ProtocolArguments
    {
        /// <summary>
        /// Memory reference to the base location from which data should be read.
        /// </summary>
        public string MemoryReference { get; set; }

        /// <summary>
        /// Optional offset (in bytes) to be applied to the reference location before reading data. Can be negative.
        /// </summary>
        public long? Offset { get; set; }

        /// <summary>
        /// Number of bytes to read at the specified location and offset.
        /// </summary>
        public int Count { get; set; }
    }
}
