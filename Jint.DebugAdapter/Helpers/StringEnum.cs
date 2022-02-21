/*
StringEnum idea and implementation based on:
https://github.com/octokit/octokit.net/blob/main/Octokit/Models/Response/StringEnum.cs

Copyright (c) 2021 Jither
Copyright (c) 2017 GitHub, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in 
the Software without restriction, including without limitation the rights to 
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Jint.DebugAdapter.Helpers
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct StringEnum<TEnum> : IEquatable<StringEnum<TEnum>> where TEnum : struct, Enum
    {
        private readonly string stringValue;
        private TEnum? value;

        public StringEnum(TEnum value)
        {
            if (!Enum.IsDefined(value))
            {
                throw new ArgumentException($"Invalid StringEnum value - must be defined on enum. Was: {value}", nameof(value));
            }

            stringValue = EnumNameHelper.ValueToName(value);
            this.value = value;
        }

        public StringEnum(string stringValue)
        {
            if (String.IsNullOrEmpty(stringValue))
            {
                throw new ArgumentNullException(nameof(stringValue), "StringEnum cannot be null or empty string");
            }
            this.stringValue = stringValue;
            value = null;
        }

        public string StringValue => stringValue;

        public TEnum Value => value ?? (value = ParseValue()).Value;

        internal string DebuggerDisplay
        {
            get
            {
                if (TryParse(out var value))
                {
                    return value.ToString();
                }
                return StringValue;
            }
        }

        public bool TryParse(out TEnum value)
        {
            if (this.value.HasValue)
            {
                value = this.value.Value;
                return true;
            }

            try
            {
                value = EnumNameHelper.NameToValue<TEnum>(StringValue);
                this.value = value;
                return true;
            }
            catch (ArgumentException)
            {
                value = default;
                return false;
            }
        }

        public bool Equals(StringEnum<TEnum> other)
        {
            if (TryParse(out var value) && other.TryParse(out var otherValue))
            {
                return value.Equals(otherValue);
            }

            return String.Equals(StringValue, other.StringValue, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is StringEnum<TEnum> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(StringValue);
        }

        public static bool operator ==(StringEnum<TEnum> left, StringEnum<TEnum> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringEnum<TEnum> left, StringEnum<TEnum> right)
        {
            return !left.Equals(right);
        }

        public static implicit operator StringEnum<TEnum>(string value)
        {
            return new StringEnum<TEnum>(value);
        }

        public static implicit operator StringEnum<TEnum>(TEnum value)
        {
            return new StringEnum<TEnum>(value);
        }

        public override string ToString()
        {
            return StringValue;
        }

        private TEnum ParseValue()
        {
            if (TryParse(out var value))
            {
                return value;
            }

            throw new ArgumentException($"Value '{value}' is not a valid {typeof(TEnum).Name} value");
        }
    }
}
