﻿using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class AttachArguments : ProtocolArguments
    {
        [JsonPropertyName("__restart")]
        public object Restart { get; set; }
    }
}
