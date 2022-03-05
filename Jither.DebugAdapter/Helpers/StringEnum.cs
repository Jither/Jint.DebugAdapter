namespace Jither.DebugAdapter.Helpers
{
    public abstract class StringEnum<T> : IEquatable<T> where T: StringEnum<T>, new()
    {
        public string EnumValue { get; private set; }
        public bool IsCustom { get; private set; }

        private static readonly Dictionary<string, T> values = new();

        protected StringEnum()
        {

        }

        protected static T Create(string value)
        {
            if (value == null)
            {
                return null;
            }

            EnsureUnique(value);

            var result = new T() { EnumValue = value, IsCustom = false };
            values.Add(value, result);

            return result;
        }

        public static T Custom(string value)
        {
            if (value == null)
            {
                return null;
            }

            EnsureUnique(value);

            return new T() { EnumValue = value, IsCustom = true };
        }

        private static void EnsureUnique(string value)
        {
            if (values.ContainsKey(value))
            {
                throw new ArgumentException($"'{value}' is already defined on {typeof(T)}");
            }
        }

        public override string ToString()
        {
            return EnumValue;
        }

        public static T Parse(string value)
        {
            if (!TryParse(value, out var result))
            {
                throw new ArgumentException($"{value} is not a valid value for the StringEnum {typeof(T).Name}", nameof(value));
            }
            return result;
        }

        public static bool TryParse(string value, out T result)
        {
            return values.TryGetValue(value, out result);
        }

        public bool Equals(T other)
        {
            return other?.EnumValue == EnumValue;
        }

        public override bool Equals(object obj)
        {
            return EnumValue.Equals((obj as T)?.EnumValue ?? (obj as string));
        }

        public override int GetHashCode()
        {
            return EnumValue.GetHashCode();
        }

        public static bool operator ==(StringEnum<T> a, StringEnum<T> b)
        {
            return a?.Equals(b) ?? false;
        }

        public static bool operator !=(StringEnum<T> a, StringEnum<T> b)
        {
            return !(a?.Equals(b) ?? false);
        }


    }
}
