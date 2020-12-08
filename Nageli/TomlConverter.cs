using System;
using Tomlyn.Model;

namespace Nageli
{
    public abstract class TomlConverter : TomlConverterFactory
    {
        public override TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options) => this;

        public abstract object ConvertFrom(TomlObject value, Type typeToConvert, TomlSerializerOptions options);

        public virtual object ConvertFromAbsent(Type typeToConvert, TomlSerializerOptions options)
            => throw new TomlException();

        public abstract TomlObject ConvertTo(object value, TomlSerializerOptions options);
    }
}
