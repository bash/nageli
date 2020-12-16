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
            var variantsMetadata = GetVariants(typeToConvert)
                .Select(variantType => CreateVariantMetadata(options, variantType))
                .ToImmutableDictionary(v => v.Tag);

            var metadata = new TaggedUnionMetadata(
                UnionType: typeToConvert,
                TagKey: GetTagKey(typeToConvert, options),
                VariantsByTag: variantsMetadata);

            return (TomlConverter)Activator.CreateInstance(
                typeof(TaggedUnionConverter<>).MakeGenericType(typeToConvert),
                metadata)!;
        }

        private static IEnumerable<Type> GetVariants(Type typeToConvert)
            => typeToConvert.GetNestedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.BaseType == typeToConvert);

        private TaggedUnionVariantMetadata CreateVariantMetadata(TomlSerializerOptions options, Type variantType)
        {
            var tag = variantType.GetCustomAttribute<TomlRenameAttribute>()?.Value ?? variantType.Name;
            var tagAdjustedToNaming = _taggedUnionOptions.TagNamingPolicy.ConvertName(tag);
            return new TaggedUnionVariantMetadata(
                VariantType: variantType,
                Converter: options.GetConverter(variantType),
                Tag: tagAdjustedToNaming);
        }

        private static string GetTagKey(Type typeToConvert, TomlSerializerOptions options)
        {
            var taggedUnionAttribute = typeToConvert.GetCustomAttribute<TomlTaggedUnionAttribute>()!;
            var tagKey = taggedUnionAttribute.Tag ?? DefaultTagKey;
            return options.PropertyNamingPolicy.ConvertName(tagKey);
        }
    }
}
