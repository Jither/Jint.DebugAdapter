using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Helpers
{
    /// <summary>
    /// StringEnum converter, tailored for DebugAdapter protocol.
    /// </summary>
    public class StringEnumConverter : JsonConverterFactory
    {
        public StringEnumConverter()
        {
            
        }

        public override bool CanConvert(Type typeToConvert)
        {
            while (typeToConvert != null)
            {
                if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(StringEnum<>))
                {
                    return true;
                }
                typeToConvert = typeToConvert.BaseType;
            }

            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return Activator.CreateInstance(typeof(Converter<>).MakeGenericType(typeToConvert)) as JsonConverter;
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

        private class Converter<T> : JsonConverter<StringEnum<T>> where T : StringEnum<T>, new()
        {
            public Converter()
            {
            }

            public override StringEnum<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException("Expected string value");
                }
                string value = reader.GetString();
                if (StringEnum<T>.TryParse(value, out T result))
                {
                    return result;
                }

                return StringEnum<T>.Custom(value);
            }

            public override void Write(Utf8JsonWriter writer, StringEnum<T> value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.EnumValue);
            }
        }
    }
}
