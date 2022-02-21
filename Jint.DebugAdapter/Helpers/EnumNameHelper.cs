using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Helpers
{
    public static class EnumNameHelper
    {
        private static readonly JsonNamingPolicy namingPolicy = JsonNamingPolicy.CamelCase;
        private static readonly Dictionary<Type, Dictionary<string, Enum>> cache = new();

        public static string ValueToName<TEnum>(TEnum value) where TEnum: struct, Enum
        {
            string name = Enum.GetName(value);
            FieldInfo field = typeof(TEnum).GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name ?? ConvertName(name);
        }

        public static TEnum NameToValue<TEnum>(string name) where TEnum: struct, Enum
        {
            var lookup = GetNamesToValues<TEnum>();
            if (!lookup.TryGetValue(name, out var result))
            {
                throw new ArgumentException($"Name '{name}' does not have a corresponding value in enum {typeof(TEnum).Name}");
            }
            return (TEnum)result;
        }

        public static string ConvertName(string name)
        {
            return namingPolicy.ConvertName(name) ?? name;
        }

        private static Dictionary<string, Enum> GetNamesToValues<TEnum>() where TEnum: struct, Enum
        {
            var type = typeof(TEnum);
            if (cache.TryGetValue(type, out var result))
            {
                return result;
            }

            var values = type.GetEnumValues();

            result = new Dictionary<string, Enum>();

            foreach (var value in values)
            {
                var typedValue = (TEnum)value;
                string transformedName = ValueToName(typedValue);
                result.Add(transformedName, typedValue);
            }
            cache.Add(type, result);
            return result;
        }
    }
}
