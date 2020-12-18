using System.Diagnostics.Contracts;

namespace Nageli.Features.NewType
{
    public static class TomlSerializerOptionsExtensions
    {
        [Pure]
        public static TomlSerializerOptions AddNewTypeConverter(this TomlSerializerOptions options)
            => options.AddConverter(new NewTypeConverterFactory());
    }
}
