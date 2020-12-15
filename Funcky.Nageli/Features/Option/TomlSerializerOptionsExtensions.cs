using System.Diagnostics.Contracts;

namespace Nageli.Features.Option
{
    public static class TomlSerializerOptionsExtensions
    {
        [Pure]
        public static TomlSerializerOptions AddOptionConverter(this TomlSerializerOptions options)
            => options.AddConverter(new OptionConverterFactory());
    }
}
