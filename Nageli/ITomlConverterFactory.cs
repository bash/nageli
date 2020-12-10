using System;

namespace Nageli
{
    public interface ITomlConverterFactory
    {
        bool CanConvert(Type type);

        TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options);
    }
}
