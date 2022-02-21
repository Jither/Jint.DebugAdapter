namespace Jint.DebugAdapter.Protocol.Types
{
    public class ExceptionBreakpointsFilter
    {
        public string Filter { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool? Default { get; set; }
        public bool? SupportsCondition { get; set; }
        public string ConditionDescription { get; set; }
    }
}
