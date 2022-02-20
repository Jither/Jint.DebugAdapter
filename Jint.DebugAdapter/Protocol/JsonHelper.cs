using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol
{
    internal static class JsonHelper
    {
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private static readonly JsonSerializerOptions outputOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        static JsonHelper()
        {
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            outputOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static string SerializeForOutput<T>(T obj)
        {
            return JsonSerializer.Serialize<object>(obj, outputOptions);
        }

        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize<object>(obj, options);
        }
    }
}
