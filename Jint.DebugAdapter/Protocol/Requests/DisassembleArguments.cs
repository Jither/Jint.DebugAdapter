namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Disassembles code stored at the provided location.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsDisassembleRequest’ is true.
    /// </remarks>
    public class DisassembleArguments : ProtocolArguments
    {
        /// <summary>
        /// Memory reference to the base location containing the instructions to disassemble.
        /// </summary>
        public string MemoryReference { get; set; }

        /// <summary>
        /// Optional offset (in bytes) to be applied to the reference location before disassembling. Can be negative.
        /// </summary>
        public long? Offset { get; set; }

        /// <summary>
        /// Optional offset (in instructions) to be applied after the byte offset (if any) before disassembling.
        /// Can be negative.
        /// </summary>
        public long? InstructionOffset { get; set; }

        /// <summary>
        /// Number of instructions to disassemble starting at the specified location and offset.
        /// </summary>
        /// <remarks>
        /// An adapter must return exactly this number of instructions - any unavailable instructions should be
        /// replaced with an implementation-defined 'invalid instruction' value.
        /// </remarks>
        public int InstructionCount { get; set; }

        /// <summary>
        /// If true, the adapter should attempt to resolve memory addresses and other values to symbolic names.
        /// </summary>
        public bool? ResolveSymbols { get; set; }
    }
}
