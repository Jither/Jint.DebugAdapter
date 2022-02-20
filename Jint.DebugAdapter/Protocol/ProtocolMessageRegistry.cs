using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Events;
using Jint.DebugAdapter.Protocol.Requests;
using Jint.DebugAdapter.Protocol.Responses;

namespace Jint.DebugAdapter.Protocol
{
    internal static class ProtocolMessageRegistry
    {
        private static readonly Dictionary<string, Type> requests = new();
        private static readonly Dictionary<string, Type> responses = new();
        private static readonly Dictionary<string, Type> events = new();

        static ProtocolMessageRegistry()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract);
            var argumentTypes = types.Where(t => typeof(ProtocolArguments).IsAssignableFrom(t));
            foreach (var type in argumentTypes)
            {
                RegisterRequest(type);
            }

            var responseBodyTypes = types.Where(t => typeof(ProtocolResponseBody).IsAssignableFrom(t));
            foreach (var type in responseBodyTypes)
            {
                RegisterResponse(type);
            }

            var eventBodyTypes = types.Where(t => typeof(ProtocolEventBody).IsAssignableFrom(t));
            foreach (var type in eventBodyTypes)
            {
                RegisterEvent(type);
            }
        }

        private static void RegisterRequest(Type type)
        {
            // Using convention for argument command name: Camel-cased type name with Arguments suffix removed.
            string command = type.Name.Replace("Arguments", String.Empty);
            command = Char.ToLowerInvariant(command[0]) + command[1..];
            var requestType = typeof(IncomingProtocolRequest<>).MakeGenericType(type);
            requests.Add(command, requestType);
        }

        private static void RegisterResponse(Type type)
        {
            // Using convention for response body command name: Camel-cased type name with ResponseBody suffix removed.
            string command = type.Name.Replace("ResponseBody", String.Empty);
            command = Char.ToLowerInvariant(command[0]) + command[1..];
            var responseType = typeof(IncomingProtocolResponse<>).MakeGenericType(type);
            responses.Add(command, responseType);
        }

        private static void RegisterEvent(Type type)
        {
            // Using convention for event body event name: Camel-cased type name with EventBody suffix removed.
            string command = type.Name.Replace("EventBody", String.Empty);
            command = Char.ToLowerInvariant(command[0]) + command[1..];
            var eventType = typeof(IncomingProtocolEvent<>).MakeGenericType(type);
            events.Add(command, eventType);
        }

        public static Type GetRequestType(string command)
        {
            return requests.GetValueOrDefault(command) ?? throw new NotSupportedException($"Unsupported request command: {command}");
        }

        public static Type GetResponseType(string command)
        {
            return responses.GetValueOrDefault(command) ?? throw new NotSupportedException($"Unsupported response command: {command}");
        }

        public static Type GetEventType(string evt)
        {
            return events.GetValueOrDefault(evt) ?? throw new NotSupportedException($"Unsupported event type: {evt}");
        }
    }
}
