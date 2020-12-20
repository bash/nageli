using System;

namespace Nageli
{
    public interface ITomlSerializerContext
    {
        TomlSerializerOptions Options { get; }

        ITomlConverter GetConverter(Type typeToConvert);

        Type? GetDefaultImplementation(Type typeToConvert);

        string ConvertPropertyName(string propertyName);
    }
}
