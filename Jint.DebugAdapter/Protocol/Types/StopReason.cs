using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal enum StopReason
    {
        Other,
        Step,
        Breakpoint,
        Exception,
        Pause,
        Entry,
        Goto,
        [JsonPropertyName("function breakpoint")]
        FunctionBreakpoint,
        [JsonPropertyName("data breakpoint")]
        DataBreakpoint,
        [JsonPropertyName("instruction breakpoint")]
        InstructionBreakpoint,
    }
}
