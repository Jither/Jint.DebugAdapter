using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// This launch request is sent from the client to the debug adapter to start the debuggee with or without debugging (if ‘noDebug’ is true).
    /// </summary>
    /// <remarks>
    /// Since launching is debugger/runtime specific, the arguments for this request are not part of this specification.
    /// </remarks>
    public class LaunchArguments : ProtocolArguments
    {
        /// <summary>
        /// If noDebug is true the launch request should launch the program without enabling debugging.
        /// </summary>
        public bool? NoDebug { get; set; }

        /// <summary>
        /// Optional data from the previous, restarted session. The data is sent as the 'restart' attribute of the 'terminated' event. The client should leave the data intact.
        /// </summary>
        [JsonPropertyName("__restart")]
        public object Restart { get; set; }
    }
}
