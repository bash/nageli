using System;
using Tomlyn.Model;

namespace Nageli
{
    /// <remarks>Do not implement this type directly. Implement <see cref="TomlConverter{T}"/> instead.</remarks>
    public abstract class TomlConverter
    {
        internal TomlConverter()
        {
        }

        public abstract object ConvertFrom(TomlObject value, Type typeToConvert, TomlSerializerOptions options);

        public abstract object ConvertFromAbsent(Type typeToConvert, TomlSerializerOptions options);

        public abstract TomlObject ConvertTo(object value, TomlSerializerOptions options);
    }
}
