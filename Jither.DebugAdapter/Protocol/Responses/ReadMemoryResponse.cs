namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘readMemory’ request.
    /// </summary>
    public class ReadMemoryResponse : ProtocolResponseBody
    {
        /// <param name="address">The address of the first byte of data returned. Treated as a hex value if
        /// prefixed with '0x', or as a decimal value otherwise.</param>
        public ReadMemoryResponse(string address)
        {
            Address = address;
        }

        /// <summary>
        /// The address of the first byte of data returned. Treated as a hex value if prefixed with '0x',
        /// or as a decimal value otherwise.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The number of unreadable bytes encountered after the last successfully read byte.
        /// </summary>
        /// <remarks>
        /// This can be used to determine the number of bytes that must be skipped before a subsequent 'readMemory'
        /// request will succeed.
        /// </remarks>
        public long? UnreadableBytes { get; set; }

        /// <summary>
        /// The bytes read from memory, encoded using base64.
        /// </summary>
        public string Data { get; set; }
    }
}
