﻿namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class CompletionsArguments : ProtocolArguments
    {
        public int? FrameId { get; set; }
        public string Text { get; set; }
        public int Column { get; set; }
        public int? Line { get; set; }
    }
}
