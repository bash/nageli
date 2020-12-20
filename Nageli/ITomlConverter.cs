using Tomlyn.Model;

namespace Nageli
{
    /// <remarks>Do not implement this interface directly. Implement <see cref="ITomlConverter{T}"/> instead.</remarks>
    public interface ITomlConverter
    {
        object ConvertFrom(TomlObject value, ITomlSerializerContext context);

        object? ConvertFromAbsent(ITomlSerializerContext context);

        TomlObject ConvertTo(object value, ITomlSerializerContext context);

        internal void DisallowDirectImplementations();
    }
}
