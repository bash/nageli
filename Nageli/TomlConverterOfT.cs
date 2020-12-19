using System;
using Tomlyn.Model;

namespace Nageli
{
    public interface ITomlConverter<T> : ITomlConverter
    {
        new T ConvertFrom(TomlObject value, TomlSerializerOptions options);

        new T ConvertFromAbsent(TomlSerializerOptions options)
            => options.AbsentValuesPolicy switch
            {
                AbsentValuesPolicy.Disallow => throw new TomlException(),
                AbsentValuesPolicy.UseDefault => default!,
                _ => throw new NotSupportedException(),
            };

        TomlObject ConvertTo(T value, TomlSerializerOptions options);

        object ITomlConverter.ConvertFrom(TomlObject value, TomlSerializerOptions options) => ConvertFrom(value, options)!;

        object ITomlConverter.ConvertFromAbsent(TomlSerializerOptions options) => ConvertFromAbsent(options)!;

        TomlObject ITomlConverter.ConvertTo(object value, TomlSerializerOptions options) => ConvertTo((T)value, options);

        void ITomlConverter.DisallowDirectImplementations()
        {
        }
    }
}
