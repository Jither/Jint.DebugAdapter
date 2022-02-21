namespace Jint.DebugAdapter.Protocol.Types
{
    public class BreakpointLocation
    {
        public int Line { get; set; }
        public int? Column { get; set; }
        public int? EndLine { get; set; }
        public int? EndColumn { get; set; }
    }
}
