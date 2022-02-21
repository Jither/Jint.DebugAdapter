using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class LaunchArguments : ProtocolArguments
    {
        public bool? NoDebug { get; set; }
        [JsonPropertyName("__restart")]
        public object Restart { get; set; }
    }
}
