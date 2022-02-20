using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        // TODO: Need custom serialization of these
        [EnumMember(Value = "function breakpoint")]
        FunctionBreakpoint,
        [EnumMember(Value = "data breakpoint")]
        DataBreakpoint,
        [EnumMember(Value = "instruction breakpoint")]
        InstructionBreakpoint,
    }
}
