using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The ‘initialize’ request is sent as the first request from the client to the debug adapter
    /// in order to configure it with client capabilities and to retrieve capabilities from the debug adapter.
    /// </summary>
    /// <remarks>
    /// Until the debug adapter has responded to with an ‘initialize’ response, the client must not
    /// send any additional requests or events to the debug adapter.
    ///     
    /// In addition the debug adapter is not allowed to send any requests or events to the client until
    /// it has responded with an ‘initialize’ response.
    ///     
    /// The ‘initialize’ request may only be sent once.
    /// </remarks>
    public class InitializeArguments : ProtocolArguments
    {
        /// <summary>
        /// The ID of the (frontend) client using this adapter.
        /// </summary>
        [JsonPropertyName("clientID")]
        public string ClientId { get; set; }

        /// <summary>
        /// The human readable name of the (frontend) client using this adapter.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// The ID of the debug adapter.
        /// </summary>
        [JsonPropertyName("adapterID")]
        public string AdapterId { get; set; }

        /// <summary>
        /// The ISO-639 locale of the (frontend) client using this adapter, e.g. en-US or de-CH.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// If true all line numbers are 1-based (default).
        /// </summary>
        public bool? LinesStartAt1 { get; set; }

        /// <summary>
        /// If true all column numbers are 1-based (default).
        /// </summary>
        public bool? ColumnsStartAt1 { get; set; }

        /// <summary>
        /// Determines in what format paths are specified. The default is 'path', which is the native format.
        /// </summary>
        public PathFormat PathFormat { get; set; }

        /// <summary>
        /// Client supports the optional type attribute for variables.
        /// </summary>
        public bool? SupportsVariableType { get; set; }

        /// <summary>
        /// Client supports the paging of variables.
        /// </summary>
        public bool? SupportsVariablePaging { get; set; }

        /// <summary>
        /// Client supports the runInTerminal request.
        /// </summary>
        public bool? SupportsRunInTerminalRequest { get; set; }

        /// <summary>
        /// Client supports memory references.
        /// </summary>
        public bool? SupportsMemoryReferences { get; set; }

        /// <summary>
        /// Client supports progress reporting.
        /// </summary>
        public bool? SupportsProgressReporting { get; set; }

        /// <summary>
        /// Client supports the invalidated event.
        /// </summary>
        public bool? SupportsInvalidatedEvent { get; set; }

        /// <summary>
        /// Client supports the memory event.
        /// </summary>
        public bool? SupportsMemoryEvent { get; set; }
    }
}
