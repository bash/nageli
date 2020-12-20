using System;

namespace Nageli
{
    internal sealed class GenericConverterFactory<T> : ITomlConverterFactory
    {
        private readonly ITomlConverter<T> _converter;

        public GenericConverterFactory(ITomlConverter<T> converter) => _converter = converter;

        public bool CanConvert(Type type) => type == typeof(T);

        public ITomlConverter CreateConverter(Type typeToConvert, ITomlSerializerContext context) => _converter;
    }
}
