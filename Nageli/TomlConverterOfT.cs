using System;
using Tomlyn.Model;

namespace Nageli
{
    public abstract class TomlConverter<T> : TomlConverter
        where T : notnull
    {
        public override bool CanConvert(Type type) => typeof(T) == type;

        public override object ConvertFrom(TomlObject value, Type typeToConvert, TomlSerializerOptions options)
            => ConvertFrom(value, options);
        
        public override object ConvertFromAbsent(Type typeToConvert, TomlSerializerOptions options)
            => ConvertFromAbsent(options);

        public override TomlObject ConvertTo(object value, TomlSerializerOptions options)
            => ConvertTo((T)value, options);
        
        public virtual T ConvertFromAbsent(TomlSerializerOptions options)
            => throw new TomlException();

        public abstract T ConvertFrom(TomlObject value, TomlSerializerOptions options);

        public abstract TomlObject ConvertTo(T value, TomlSerializerOptions options);
    }
}
