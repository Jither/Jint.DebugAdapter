﻿using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class StepInArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public bool? SingleThread { get; set; }
        public int? TargetId { get; set; }
        public StringEnum<SteppingGranularity>? Granularity { get; set; }
    }
}