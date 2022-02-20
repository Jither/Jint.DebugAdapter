namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class DisassembleArguments : ProtocolArguments
    {
        public string MemoryReference { get; set; }
        public long? Offset { get; set; }
        public long? InstructionOffset { get; set; }
        public int InstructionCount { get; set; }
        public bool? ResolveSymbols { get; set; }
    }
}
