using System.Diagnostics.CodeAnalysis;
using Tomlyn.Model;

namespace Nageli
{
    public interface ITomlConverter<T> : ITomlConverter
    {
        new T ConvertFrom(TomlObject value, ITomlSerializerContext context);

        [return: MaybeNull]
        new T ConvertFromAbsent(ITomlSerializerContext context)
            => context.Options.AbsentValuesPolicy.ConvertFromAbsent<T>(context);

        TomlObject ConvertTo(T value, ITomlSerializerContext context);

        object ITomlConverter.ConvertFrom(TomlObject value, ITomlSerializerContext context) => ConvertFrom(value, context)!;

        object? ITomlConverter.ConvertFromAbsent(ITomlSerializerContext context) => ConvertFromAbsent(context)!;

        TomlObject ITomlConverter.ConvertTo(object value, ITomlSerializerContext context) => ConvertTo((T)value, context);

        void ITomlConverter.DisallowDirectImplementations()
        {
        }
    }
}
