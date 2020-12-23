using System;
using Nageli.Model;

namespace Nageli.Converters
{
    internal sealed class SimpleConverter<T> : ITomlConverter<T>
        where T : IEquatable<T>
    {
        public T ConvertFrom(TomlObject value, ITomlSerializerContext context)
            => value is TomlValue<T> tomlValue
                ? tomlValue.Value
                : throw new TomlException();

        public TomlObject ConvertTo(T value, ITomlSerializerContext context) => ToTomlObject(value);

        private static TomlObject ToTomlObject(object value)
            => value switch
            {
                string @string => new TomlString(@string),
                long @long => new TomlInteger(@long),
                bool @bool => new TomlBoolean(@bool),
                double @double => new TomlFloat(@double),
                DateTime dateTime => new TomlDateTime(dateTime),
                DateTimeOffset dateTimeOffset => new TomlLocalDateTime(dateTimeOffset, DateTimeMask.None),
                _ => throw new InvalidOperationException("Unreachable branch"),
            };
    }
}
