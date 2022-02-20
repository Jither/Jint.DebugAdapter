﻿using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class GotoTargetsArguments : ProtocolArguments
    {
        public Source Source { get; set; }
        public int Line { get; set; }
        public int? Column { get; set; }
    }
}
