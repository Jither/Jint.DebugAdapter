namespace Jint.DebugAdapter.Protocol.Types
{
    internal class InstructionBreakpoint
    {
        public string InstructionReference { get; set; }
        public long? Offset { get; set; }
        public string Condition { get; set; }
        public string HitCondition { get; set; }
    }
}
