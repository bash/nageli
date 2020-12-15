using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Nageli.Features.TaggedUnion
{
    internal sealed class TaggedUnionConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type)
            => Attribute.IsDefined(type, attributeType: typeof(TomlTaggedUnionAttribute)) && type.IsAbstract;

        public TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
        {
            var taggedUnionAttribute = typeToConvert.GetCustomAttribute<TomlTaggedUnionAttribute>()!;
            var variantsMetadata = typeToConvert.GetNestedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => Attribute.IsDefined(t, typeof(TomlTagAttribute)))
                .Where(t => t.BaseType == typeToConvert)
                .Select(variantType => new TaggedUnionVariantMetadata(
                    VariantType: variantType,
                    Converter: options.GetConverter(variantType),
                    Tag: variantType.GetCustomAttribute<TomlTagAttribute>()!.Value))
                .ToImmutableDictionary(t => t.Tag);

            var metadata = new TaggedUnionMetadata(
                UnionType: typeToConvert,
                TagKey: taggedUnionAttribute.Tag,
                VariantsByTag: variantsMetadata);

            return (TomlConverter)Activator.CreateInstance(
                typeof(TaggedUnionConverter<>).MakeGenericType(typeToConvert),
                metadata)!;
        }
    }

    internal sealed record TaggedUnionMetadata(
        Type UnionType,
        string TagKey,
        IReadOnlyDictionary<string, TaggedUnionVariantMetadata> VariantsByTag)
    {
    }

    internal sealed record TaggedUnionVariantMetadata(
        Type VariantType,
        string Tag,
        TomlConverter Converter)
    {
    }
}
