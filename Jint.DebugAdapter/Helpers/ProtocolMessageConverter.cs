using System.Text.Json;
using System.Text.Json.Serialization;
using Jint.DebugAdapter.Protocol;
using Jint.DebugAdapter.Protocol.Requests;

namespace Jint.DebugAdapter.Helpers
{
    internal class ProtocolMessageConverter : JsonConverter<ProtocolMessage>
    {
        public override ProtocolMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected protocol message to be a JSON object");
            }

            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                if (!doc.RootElement.TryGetProperty("type", out var typeName))
                {
                    throw new JsonException($"Protocol message type not found");
                }

                string command = null;
                string evt = null;
                if (doc.RootElement.TryGetProperty("command", out var commandProp))
                {
                    command = commandProp.GetString();
                }
                if (doc.RootElement.TryGetProperty("event", out var evtProp))
                {
                    evt = evtProp.GetString();
                }

                var type = GetConcreteType(typeName.GetString(), command, evt);

                var jsonObject = doc.RootElement.GetRawText();
                var result = JsonSerializer.Deserialize(jsonObject, type, options) as ProtocolMessage;

                // For equal treatment when dealing with argument-less requests, instantiantiate empty arguments object 
                if (result is IncomingProtocolRequest req && req.UntypedArguments == null)
                {
                    var argumentsType = ProtocolMessageRegistry.GetArgumentsType(command);
                    req.Sanitize(Activator.CreateInstance(argumentsType) as ProtocolArguments);
                }

                return result;
            }
        }

        private Type GetConcreteType(string typeName, string command, string evt)
        {
            return typeName switch
            {
                BaseProtocolRequest.TypeName => ProtocolMessageRegistry.GetRequestType(command),
                BaseProtocolResponse.TypeName => ProtocolMessageRegistry.GetResponseType(command),
                BaseProtocolEvent.TypeName => ProtocolMessageRegistry.GetEventType(evt),
                _ => throw new NotSupportedException($"Unsupported protocol message type: {typeName}"),
            };
        }

        public override void Write(Utf8JsonWriter writer, ProtocolMessage value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
