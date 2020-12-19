using System;

namespace Nageli
{
    public interface ITomlConverterFactory
    {
        bool CanConvert(Type type);

        ITomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options);
    }
}
