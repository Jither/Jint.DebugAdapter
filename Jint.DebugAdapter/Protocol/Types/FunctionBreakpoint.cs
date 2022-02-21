namespace Jint.DebugAdapter.Protocol.Types
{
    public class FunctionBreakpoint
    {
        public string Name { get; set; }
        public string Condition { get; set; }
        public string HitCondition { get; set; }
    }
}
