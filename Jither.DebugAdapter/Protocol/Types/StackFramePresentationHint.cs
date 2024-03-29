﻿using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="StackFramePresentationHint"/>
    public class StackFramePresentationHint : StringEnum<StackFramePresentationHint>
    {
        public static readonly StackFramePresentationHint Normal = Create("normal");
        public static readonly StackFramePresentationHint Label = Create("label");
        public static readonly StackFramePresentationHint Subtle = Create("subtle");
    }
}
