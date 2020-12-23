using Nageli.Model;

namespace Nageli.Features.TaggedUnion
{
    internal class TaggedUnionConverter<T> : ITomlConverter<T>
    {
        private readonly TaggedUnionMetadata _metadata;

        public TaggedUnionConverter(TaggedUnionMetadata metadata) => _metadata = metadata;

        public T ConvertFrom(TomlObject value, ITomlSerializerContext context)
            => value is TomlTable table
                ? ConvertFrom(table, context)
                : throw new TomlException();

        public TomlObject ConvertTo(T value, ITomlSerializerContext context) => throw new System.NotImplementedException();

        private T ConvertFrom(TomlTable table, ITomlSerializerContext context)
        {
            if (!table.TryGetValue(_metadata.TagKey, out var tag))
            {
                throw new TomlException($"Missing key \"{_metadata.TagKey}\" in table");
            }

            if (tag is not TomlString tagString)
            {
                throw new TomlException($"Unexpected type \"{tag.GetType()}\" for \"{_metadata.TagKey}\", must be a string");
            }

            if (!_metadata.VariantsByTag.TryGetValue(tagString.Value, out var variant))
            {
                throw new TomlException($"\"{tagString.Value}\" is not a valid value for \"{_metadata.TagKey}\"");
            }

            return (T)variant.Converter.ConvertFrom(table, context);
        }
    }
}
