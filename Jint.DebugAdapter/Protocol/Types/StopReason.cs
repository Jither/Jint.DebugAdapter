using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="StopReason"/>
    public class StopReason : StringEnum<StopReason>
    {
        public static readonly StopReason Step = Create("step");
        public static readonly StopReason Breakpoint = Create("breakpoint");
        public static readonly StopReason Exception = Create("exception");
        public static readonly StopReason Pause = Create("pause");
        public static readonly StopReason Entry = Create("entry");
        public static readonly StopReason Goto = Create("goto");
        public static readonly StopReason FunctionBreakpoint = Create("function breakpoint");
        public static readonly StopReason DataBreakpoint = Create("data breakpoint");
        public static readonly StopReason InstructionBreakpoint = Create("instruction breakpoint");
    }
}
