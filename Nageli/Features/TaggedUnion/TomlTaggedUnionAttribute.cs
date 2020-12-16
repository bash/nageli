using System;

namespace Nageli.Features.TaggedUnion
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TomlTaggedUnionAttribute : Attribute
    {
        public string? Tag { get; init; }
    }
}
