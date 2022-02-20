using System.Text.Json.Serialization;
using Jint.DebugAdapter.Protocol.Responses;

namespace Jint.DebugAdapter.Protocol
{
    internal abstract class BaseProtocolResponse : ProtocolMessage
    {
        public const string TypeName = "response";

        [JsonPropertyOrder(-10)]
        public string Command { get; set; }

        [JsonPropertyOrder(-9)]
        public bool Success { get; set; }

        [JsonPropertyName("request_seq")]
        public int RequestSeq { get; set; }
        
        
        public string Message { get; set; }
        
        public abstract ProtocolResponseBody UntypedBody { get; }

        public BaseProtocolResponse()
        {
            Type = TypeName;
        }
    }

    internal class ProtocolResponse : BaseProtocolResponse
    {
        [JsonIgnore]
        public ProtocolResponseBody Body { get; private set; }
        [JsonIgnore]
        public override ProtocolResponseBody UntypedBody => Body;
        [JsonPropertyName("body"), JsonPropertyOrder(100)]
        public object SerializedBody => Body;

        public ProtocolResponse(string command, int requestSeq, bool success, ProtocolResponseBody body, string message = null)
        {
            Command = command;
            RequestSeq = requestSeq;
            Success = success;
            Body = body;
            Message = message;
        }
    }

    internal class IncomingProtocolResponse<T> : BaseProtocolResponse where T: ProtocolResponseBody
    {
        [JsonPropertyOrder(100)]
        public T Body { get; set; }

        [JsonIgnore]
        public override ProtocolResponseBody UntypedBody => Body;
    }
}
