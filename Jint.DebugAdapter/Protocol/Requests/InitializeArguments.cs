using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class InitializeArguments : ProtocolArguments
    {
        [JsonPropertyName("clientID")]
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        [JsonPropertyName("adapterID")]
        public string AdapterId { get; set; }
        public string Locale { get; set; }
        public bool? LinesStartAt1 { get; set; }
        public bool? ColumnsStartAt1 { get; set; }
        public string PathFormat { get; set; }
        public bool? SupportsVariableType { get; set; }
        public bool? SupportsVariablePaging { get; set; }
        public bool? SupportsRunInTerminalRequest { get; set; }
        public bool? SupportsMemoryReferences { get; set; }
        public bool? SupportsProgressReporting { get; set; }
        public bool? SupportsInvalidatedEvent { get; set; }
        public bool? SupportsMemoryEvent { get; set; }
    }
}
