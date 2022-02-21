using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Helpers
{
    /// <summary>
    /// StringEnum converter, tailored for DebugAdapter protocol.
    /// The StringEnum allows using Enum values for known values while still allowing arbitrary string values.
    /// </summary>
    public class StringEnumConverter : JsonConverterFactory
    {
        public StringEnumConverter()
        {
            
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            // If nullable, check underlying type instead
            bool isNullable = CheckNullableType(typeToConvert, out var underlyingType);
            if (isNullable)
            {
                typeToConvert = underlyingType;
            }

            return typeToConvert.GetGenericTypeDefinition() == typeof(StringEnum<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            bool isNullable = CheckNullableType(typeToConvert, out var underlyingType);
            try
            {
                Type enumType;
                if (isNullable)
                {
                    enumType = underlyingType.GetGenericArguments()[0];

                    return Activator.CreateInstance(typeof(NullableConverter<>).MakeGenericType(enumType)) as JsonConverter;
                }
                enumType = typeToConvert.GetGenericArguments()[0];
                return Activator.CreateInstance(typeof(Converter<>).MakeGenericType(enumType)) as JsonConverter;
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

        private static bool CheckNullableType(Type type, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType == typeof(StringEnum<>);
        }

        private class Converter<TEnum> : JsonConverter<StringEnum<TEnum>> where TEnum: struct, Enum
        {
            public Converter()
            {
            }

            public override StringEnum<TEnum> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException("Expected string value");
                }
                string value = reader.GetString();
                return new StringEnum<TEnum>(value);
            }

            public override void Write(Utf8JsonWriter writer, StringEnum<TEnum> value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.StringValue);
            }
        }

        private class NullableConverter<TEnum> : JsonConverter<StringEnum<TEnum>?> where TEnum: struct, Enum
        {
            public NullableConverter()
            {
            }

            public override StringEnum<TEnum>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.TokenType switch
                {
                    JsonTokenType.String => new StringEnum<TEnum>(reader.GetString()),
                    JsonTokenType.Null => null,
                    _ => throw new JsonException("Expected string or null value"),
                };
            }

            public override void Write(Utf8JsonWriter writer, StringEnum<TEnum>? value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    writer.WriteStringValue(value.Value.StringValue);
                }
            }
        }
    }
}
