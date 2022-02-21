namespace Jint.DebugAdapter.Protocol.Types
{
    internal class DataBreakpoint
    {
        public string DataId { get; set; }
        // This isn't a StringEnum - it has the members it's likely to ever have, and is also used in a List in DataBreakpointInfoResponse
        public DataBreakpointAccessType? AccessType { get; set; }
        public string Condition { get; set; }
        public string HitCondition { get; set; }
    }
}
