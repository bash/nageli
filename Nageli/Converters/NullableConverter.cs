using System;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class NullableConverter<T> : TomlConverter<T?>
        where T : struct
    {
        private readonly TomlConverter<T> _valueConverter;

        public NullableConverter(TomlSerializerOptions options) => _valueConverter = options.GetConverter<T>();

        public override T? ConvertFrom(TomlObject value, TomlSerializerOptions options) => _valueConverter.ConvertFrom(value, options);

        public override T? ConvertFromAbsent(TomlSerializerOptions options) => null;

        public override TomlObject ConvertTo(T? value, TomlSerializerOptions options)
            => throw new NotImplementedException();
    }
}
