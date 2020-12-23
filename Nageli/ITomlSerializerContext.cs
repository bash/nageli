using System;
using Tomlyn.Model;

namespace Nageli
{
    public interface ITomlSerializerContext : ITomlToSerializerContext
    {
        TomlSerializerOptions Options { get; }

        ITomlConverter GetConverter(Type typeToConvert);

        Type? GetDefaultImplementation(Type typeToConvert);

        string ConvertPropertyName(string propertyName);
    }
}
