using System;

namespace Nageli.Features.TaggedUnion
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TomlTagAttribute : Attribute
    {
        public TomlTagAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
