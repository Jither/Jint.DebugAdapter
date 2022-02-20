using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Events;

namespace Jint.DebugAdapter.Protocol
{
    internal abstract class BaseProtocolEvent : ProtocolMessage
    {
        public const string TypeName = "event";

        [JsonPropertyOrder(-10)]
        public string Event { get; set; }
        public abstract ProtocolEventBody UntypedBody { get; }

        protected BaseProtocolEvent()
        {
            Type = TypeName;
        }
    }

    internal class ProtocolEvent : BaseProtocolEvent
    {
        [JsonIgnore]
        public ProtocolEventBody Body { get; private set; }
        
        [JsonIgnore]
        public override ProtocolEventBody UntypedBody => Body;

        [JsonPropertyName("body"), JsonPropertyOrder(100)]
        public object SerializedBody => Body;

        public ProtocolEvent(string evt, ProtocolEventBody body)
        {
            Event = evt;
            Body = body;
        }
    }

    internal class IncomingProtocolEvent<TBody> : BaseProtocolEvent where TBody: ProtocolEventBody
    {
        [JsonPropertyOrder(100)]
        public TBody Body { get; set; }
        
        [JsonIgnore]
        public override ProtocolEventBody UntypedBody => Body;
    }
}
