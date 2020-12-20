using System;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class NullableConverter<T> : ITomlConverter<T?>
        where T : struct
    {
        private readonly ITomlConverter<T> _valueConverter;

        public NullableConverter(ITomlSerializerContext context) => _valueConverter = context.GetConverter<T>();

        public T? ConvertFrom(TomlObject value, ITomlSerializerContext context) => _valueConverter.ConvertFrom(value, context);

        public T? ConvertFromAbsent(ITomlSerializerContext context) => null;

        public TomlObject ConvertTo(T? value, ITomlSerializerContext context)
            => throw new NotImplementedException();
    }
}
