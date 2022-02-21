namespace Jint.DebugAdapter.Protocol.Types
{
    public class SourceBreakpoint
    {
        public int Line { get; set; }
        public int? Column { get; set; }
        public string Condition { get; set; }
        public string HitCondition { get; set; }
        public string LogMessage { get; set; }
    }
}
