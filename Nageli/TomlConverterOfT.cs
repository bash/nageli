using System;
using Tomlyn.Model;

namespace Nageli
{
    public interface ITomlConverter<T> : ITomlConverter
    {
        new T ConvertFrom(TomlObject value, ITomlSerializerContext context);

        // TODO: move this implementation to ITomlSerializerContext
        new T ConvertFromAbsent(ITomlSerializerContext context)
            => context.Options.AbsentValuesPolicy switch
            {
                AbsentValuesPolicy.Disallow => throw new TomlException(),
                AbsentValuesPolicy.UseDefault => default!,
                _ => throw new NotSupportedException(),
            };

        TomlObject ConvertTo(T value, ITomlSerializerContext context);

        object ITomlConverter.ConvertFrom(TomlObject value, ITomlSerializerContext context) => ConvertFrom(value, context)!;

        object ITomlConverter.ConvertFromAbsent(ITomlSerializerContext context) => ConvertFromAbsent(context)!;

        TomlObject ITomlConverter.ConvertTo(object value, ITomlSerializerContext context) => ConvertTo((T)value, context);

        void ITomlConverter.DisallowDirectImplementations()
        {
        }
    }
}
