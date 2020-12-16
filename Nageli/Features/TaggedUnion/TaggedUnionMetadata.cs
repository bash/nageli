using System;
using System.Collections.Generic;

namespace Nageli.Features.TaggedUnion
{
    internal sealed record TaggedUnionMetadata(
        Type UnionType,
        string TagKey,
        IReadOnlyDictionary<string, TaggedUnionVariantMetadata> VariantsByTag);
}
