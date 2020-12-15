using System;

namespace Nageli.Features.TaggedUnion
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TomlTaggedUnionAttribute : Attribute
    {
        public TomlTaggedUnionAttribute(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; }
    }
}
