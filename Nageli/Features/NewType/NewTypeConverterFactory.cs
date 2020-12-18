using System;

namespace Nageli.Features.NewType
{
    internal sealed class NewTypeConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type) => Attribute.IsDefined(type, typeof(TomlNewTypeAttribute));

        public TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options) => throw new NotImplementedException();
    }
}
