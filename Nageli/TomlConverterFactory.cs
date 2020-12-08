using System;

namespace Nageli
{
    public abstract class TomlConverterFactory
    {
        public abstract bool CanConvert(Type type);

        public abstract TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options);
    }
}
