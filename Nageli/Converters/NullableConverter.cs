using System;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class NullableConverter<T> : ITomlConverter<T?>
        where T : struct
    {
        private readonly ITomlConverter<T> _valueConverter;

        public NullableConverter(TomlSerializerOptions options) => _valueConverter = options.GetConverter<T>();

        public T? ConvertFrom(TomlObject value, TomlSerializerOptions options) => _valueConverter.ConvertFrom(value, options);

        public T? ConvertFromAbsent(TomlSerializerOptions options) => null;

        public TomlObject ConvertTo(T? value, TomlSerializerOptions options)
            => throw new NotImplementedException();
    }
}
