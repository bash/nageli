using System.Diagnostics.Contracts;

namespace Nageli.Features.TaggedUnion
{
    public static class TomlSerializerOptionsExtensions
    {
        [Pure]
        public static TomlSerializerOptions AddTaggedUnionConverter(this TomlSerializerOptions options, TomlTaggedUnionOptions? taggedUnionOptions = null)
            => options.AddConverter(new TaggedUnionConverterFactory(taggedUnionOptions ?? TomlTaggedUnionOptions.Default));
    }
}
