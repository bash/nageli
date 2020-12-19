using System;

namespace Nageli.Features.TaggedUnion
{
    internal sealed record TaggedUnionVariantMetadata(
        Type VariantType,
        string Tag,
        ITomlConverter Converter);
}
