using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Helpers
{
    /// <summary>
    /// Custom JSON Enum <-> string converter, tailored for DebugAdapter protocol.
    /// This allows specifying custom strings for enumeration values through JsonPropertyName attribute.
    /// Unlike the standard JsonStringEnumConverter, this always uses the CamelCasing NamingPolicy, and
    /// doesn't allow integers for enum values.
    /// </summary>
    public class CustomJsonStringEnumConverter : JsonConverterFactory
    {
        private readonly HashSet<Type> types;

        public CustomJsonStringEnumConverter()
        {
            
        }

        public CustomJsonStringEnumConverter(params Type[] targetTypes)
        {
            if (targetTypes.Length > 0)
            {
                types = new HashSet<Type>(targetTypes.Length);
                foreach (var type in targetTypes)
                {
                    if (!type.IsEnum)
                    {
                        throw new ArgumentException($"Type '{type}' is not an enum");
                    }
                    types.Add(type);
                    types.Add(typeof(Nullable<>).MakeGenericType(type));
                }
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            // If this converter is for a specific set of types:
            if (types != null)
            {
                return types.Contains(typeToConvert);
            }
            // Otherwise, any enum will do:
            if (typeToConvert.IsEnum)
            {
                return true;
            }
            // ... or any nullable enum:
            (bool isNullable, _) = TestNullableEnum(typeToConvert);
            return isNullable;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            (bool isNullable, Type underlyingType) = TestNullableEnum(typeToConvert);

            try
            {
                if (isNullable)
                {
                    return Activator.CreateInstance(typeof(NullableEnumConverter<>).MakeGenericType(underlyingType)) as JsonConverter;
                }
                return Activator.CreateInstance(typeof(EnumConverter<>).MakeGenericType(typeToConvert)) as JsonConverter;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw;
            }
        }

        private static (bool IsNullable, Type EnumType) TestNullableEnum(Type type)
        {
            Type EnumType = Nullable.GetUnderlyingType(type);
            return (EnumType?.IsEnum ?? false, EnumType);
        }

        private class EnumConverter<TEnum> : JsonConverter<TEnum> where TEnum: struct, Enum
        {
            private readonly JsonStringEnumConverterHelper<TEnum> helper;

            public EnumConverter()
            {
                helper = new JsonStringEnumConverterHelper<TEnum>();
            }

            public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return helper.Read(ref reader);
            }

            public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                helper.Write(writer, value);
            }
        }

        private class NullableEnumConverter<TEnum> : JsonConverter<TEnum?> where TEnum: struct, Enum
        {
            private readonly JsonStringEnumConverterHelper<TEnum> helper;

            public NullableEnumConverter()
            {
                helper = new JsonStringEnumConverterHelper<TEnum>();
            }

            public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return helper.Read(ref reader);
            }

            public override void Write(Utf8JsonWriter writer, TEnum? value, JsonSerializerOptions options)
            {
                helper.Write(writer, value.Value);
            }
        }
    }

    internal class JsonStringEnumConverterHelper<TEnum> where TEnum: struct, Enum
    {
        private class EnumInfo
        {
            public string Name { get; }
            public TEnum EnumValue { get; }

            public EnumInfo(string name, TEnum enumValue)
            {
                Name = name;
                EnumValue = enumValue;
            }
        }

        private const BindingFlags bindings = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

        private readonly Type enumType;
        private readonly Dictionary<TEnum, EnumInfo> rawToTransformed;
        private readonly Dictionary<string, EnumInfo> transformedToRaw;

        // For simplification, this converter *always* uses camel case for naming.
        private readonly JsonNamingPolicy namingPolicy = JsonNamingPolicy.CamelCase;

        public JsonStringEnumConverterHelper()
        {
            enumType = typeof(TEnum);

            string[] definedNames = enumType.GetEnumNames();
            Array definedValues = enumType.GetEnumValues();

            int definedCount = definedNames.Length;

            rawToTransformed = new Dictionary<TEnum, EnumInfo>(definedCount);
            transformedToRaw = new Dictionary<string, EnumInfo>(definedCount);

            for (int i = 0; i < definedCount; i++)
            {
                Enum enumValue = (Enum)definedValues.GetValue(i);
                if (enumValue is not TEnum typedValue)
                {
                    throw new NotSupportedException();
                }
                if (enumValue == null)
                {
                    continue;
                }

                string name = definedNames[i];
                FieldInfo field = enumType.GetField(name, bindings);

                string transformedName = field.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name ?? namingPolicy.ConvertName(name) ?? name;

                rawToTransformed[typedValue] = new EnumInfo(transformedName, typedValue);
                transformedToRaw[transformedName] = new EnumInfo(name, typedValue);
            }
        }

        public TEnum Read(ref Utf8JsonReader reader)
        {
            var token = reader.TokenType;

            if (token == JsonTokenType.String)
            {
                string enumString = reader.GetString();

                if (transformedToRaw.TryGetValue(enumString, out var enumInfo))
                {
                    return enumInfo.EnumValue;
                }

                foreach (var enumItem in transformedToRaw)
                {
                    if (String.Equals(enumItem.Key, enumString, StringComparison.OrdinalIgnoreCase))
                    {
                        return enumItem.Value.EnumValue;
                    }
                }
            }

            throw new JsonException($"Unable to convert value of type {enumType}");
        }

        public void Write(Utf8JsonWriter writer, TEnum value)
        {
            if (!rawToTransformed.TryGetValue(value, out var enumInfo))
            {
                throw new JsonException($"Enum type {enumType} does not have a mapping for {value}");
            }
            writer.WriteStringValue(enumInfo.Name);
        }
    }
}
