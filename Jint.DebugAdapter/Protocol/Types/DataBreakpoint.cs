namespace Jint.DebugAdapter.Protocol.Types
{
    internal class DataBreakpoint
    {
        public string DataId { get; set; }
        public DataBreakpointAccessType? AccessType { get; set; }
        public string Condition { get; set; }
        public string HitCondition { get; set; }
    }
}
