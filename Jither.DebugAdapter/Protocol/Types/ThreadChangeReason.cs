﻿using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="ThreadChangeReason"/>
    public class ThreadChangeReason : StringEnum<ThreadChangeReason>
    {
        public static readonly ThreadChangeReason Started = Create("started");
        public static readonly ThreadChangeReason Exited = Create("exited");
    }
}
