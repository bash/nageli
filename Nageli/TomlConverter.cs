using System;
using Tomlyn.Model;

namespace Nageli
{
    public abstract class TomlConverter
    {
        public abstract object ConvertFrom(TomlObject value, Type typeToConvert, TomlSerializerOptions options);

        public abstract object ConvertFromAbsent(Type typeToConvert, TomlSerializerOptions options);

        public abstract TomlObject ConvertTo(object value, TomlSerializerOptions options);
    }
}
