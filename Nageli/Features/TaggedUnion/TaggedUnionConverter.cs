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
            var tagKey = options.PropertyNamingPolicy.ConvertName(_metadata.TagKey);

            if (!table.TryGetToml(tagKey, out var tag))
            {
                throw new TomlException();
            }

            if (tag is not TomlString tagString)
            {
                throw new TomlException();
            }

            if (!_metadata.VariantsByTag.TryGetValue(tagString.Value, out var variant))
            {
                throw new TomlException();
            }

            return (T)variant.Converter.ConvertFrom(table, variant.VariantType, options);
        }
    }
}
