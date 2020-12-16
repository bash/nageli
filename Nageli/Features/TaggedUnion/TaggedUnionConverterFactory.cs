using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Nageli.Features.TaggedUnion
{
    internal sealed class TaggedUnionConverterFactory : ITomlConverterFactory
    {
        private const string DefaultTagKey = "Type";

        private readonly TomlTaggedUnionOptions _taggedUnionOptions;

        public TaggedUnionConverterFactory(TomlTaggedUnionOptions taggedUnionOptions)
            => _taggedUnionOptions = taggedUnionOptions;

        public bool CanConvert(Type type)
            => Attribute.IsDefined(type, attributeType: typeof(TomlTaggedUnionAttribute));

        public TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
        {
            var taggedUnionAttribute = typeToConvert.GetCustomAttribute<TomlTaggedUnionAttribute>()!;

            // TODO: use type name when TagAttribute is not specified.
            var variantsMetadata = typeToConvert.GetNestedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.BaseType == typeToConvert)
                .Select(variantType => new TaggedUnionVariantMetadata(
                    VariantType: variantType,
                    Converter: options.GetConverter(variantType),
                    Tag: _taggedUnionOptions.TagNamingPolicy.ConvertName(
                        variantType.GetCustomAttribute<TomlRenameAttribute>()?.Value ?? variantType.Name)))
                .ToImmutableDictionary(v => v.Tag);

            var tagKey = options.PropertyNamingPolicy.ConvertName(taggedUnionAttribute.Tag ?? DefaultTagKey);
            var metadata = new TaggedUnionMetadata(
                UnionType: typeToConvert,
                TagKey: tagKey,
                VariantsByTag: variantsMetadata);

            return (TomlConverter)Activator.CreateInstance(
                typeof(TaggedUnionConverter<>).MakeGenericType(typeToConvert),
                metadata)!;
        }
    }

    internal sealed record TaggedUnionMetadata(
        Type UnionType,
        string TagKey,
        IReadOnlyDictionary<string, TaggedUnionVariantMetadata> VariantsByTag);

    internal sealed record TaggedUnionVariantMetadata(
        Type VariantType,
        string Tag,
        TomlConverter Converter);
}
