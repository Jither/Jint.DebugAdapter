using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Base class for AttachArguments and LaunchArguments used as the Arguments property on RestartArguments
    /// </summary>
    public class ConfigurationArguments : ProtocolArguments
    {
        // This isn't actually applicable for Attach - but it will just be null for that request.
        /// <summary>
        /// If noDebug is true the launch request should launch the program without enabling debugging.
        /// </summary>
        public bool? NoDebug { get; set; }

        /// <summary>
        /// Optional data from the previous, restarted session. The data is sent as the 'restart' attribute of the
        /// 'terminated' event. The client should leave the data intact.
        /// </summary>
        [JsonPropertyName("__restart")]
        public object Restart { get; set; }
    }
}
