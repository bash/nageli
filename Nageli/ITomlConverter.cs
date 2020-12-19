using Tomlyn.Model;

namespace Nageli
{
    /// <remarks>Do not implement this interface directly. Implement <see cref="ITomlConverter{T}"/> instead.</remarks>
    public interface ITomlConverter
    {
        object ConvertFrom(TomlObject value, TomlSerializerOptions options);

        object ConvertFromAbsent(TomlSerializerOptions options);

        TomlObject ConvertTo(object value, TomlSerializerOptions options);

        internal void DisallowDirectImplementations();
    }
}
