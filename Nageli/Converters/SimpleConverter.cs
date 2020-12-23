using System;
using Nageli.Model;

namespace Nageli.Converters
{
    internal sealed class SimpleConverter<T> : ITomlConverter<T>
        where T : IEquatable<T>
    {
        public T ConvertFrom(TomlObject value, ITomlSerializerContext context)
        {
            if (value is TomlValue<T> tomlValue)
            {
                return tomlValue.Value;
            }

            throw new TomlException();
        }

        public TomlObject ConvertTo(T value, ITomlSerializerContext context) => ToTomlObject(value);

        /// Adapted from:
        /// https://github.com/xoofx/Tomlyn/blob/8f997483d3df29ee0ae217d75bad851ebc2ec0aa/src/Tomlyn/Model/TomlObject.cs
        internal static TomlObject ToTomlObject(object value)
            => value switch
            {
                TomlObject tomlObject => tomlObject,
                string @string => new TomlString(@string),
                long @long => new TomlInteger(@long),
                bool @bool => new TomlBoolean(@bool),
                double @double => new TomlFloat(@double),
                DateTime dateTime => new TomlDateTime(dateTime),
                _ => throw new InvalidOperationException($"The type `{value.GetType()}` of the object is invalid. Only long, bool, double, DateTime and TomlObject are supported"),
            };
    }
}
