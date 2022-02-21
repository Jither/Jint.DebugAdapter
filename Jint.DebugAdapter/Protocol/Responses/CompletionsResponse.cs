﻿using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class CompletionsResponse : ProtocolResponseBody
    {
        public List<CompletionItem> Targets { get; set; }
    }
}
