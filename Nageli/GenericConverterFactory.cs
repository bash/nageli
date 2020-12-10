using System;

namespace Nageli
{
    internal sealed class GenericConverterFactory<T> : ITomlConverterFactory
        where T : notnull
    {
        private readonly TomlConverter<T> _converter;

        public GenericConverterFactory(TomlConverter<T> converter) => _converter = converter;

        public bool CanConvert(Type type) => type == typeof(T);

        public TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options) => _converter;
    }
}
