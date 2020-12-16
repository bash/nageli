using Tomlyn.Model;

namespace Nageli.Features.TaggedUnion
{
    internal class TaggedUnionConverter<T> : TomlConverter<T>
    {
        private readonly TaggedUnionMetadata _metadata;

        public TaggedUnionConverter(TaggedUnionMetadata metadata) => _metadata = metadata;

        public override T ConvertFrom(TomlObject value, TomlSerializerOptions options)
            => value is TomlTable table
                ? ConvertFrom(table, options)
                : throw new TomlException();

        public override TomlObject ConvertTo(T value, TomlSerializerOptions options) => throw new System.NotImplementedException();

        private T ConvertFrom(TomlTable table, TomlSerializerOptions options)
        {
            if (!table.TryGetToml(_metadata.TagKey, out var tag))
            {
                throw new TomlException($"Missing key \"{_metadata.TagKey}\" in table");
            }

            if (tag is not TomlString tagString)
            {
                throw new TomlException($"Unexpected type \"{tag.Kind}\" for \"{_metadata.TagKey}\", must be a string");
            }

            if (!_metadata.VariantsByTag.TryGetValue(tagString.Value, out var variant))
            {
                throw new TomlException($"\"{tagString.Value}\" is not a valid value for \"{_metadata.TagKey}\"");
            }

            return (T)variant.Converter.ConvertFrom(table, variant.VariantType, options);
        }
    }
}
