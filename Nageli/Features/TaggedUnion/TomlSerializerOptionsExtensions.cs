using System.Diagnostics.Contracts;

namespace Nageli.Features.TaggedUnion
{
    public static class TomlSerializerOptionsExtensions
    {
        [Pure]
        public static TomlSerializerOptions AddTaggedUnionConverter(this TomlSerializerOptions options)
            => options.AddConverter(new TaggedUnionConverterFactory());
    }
}
