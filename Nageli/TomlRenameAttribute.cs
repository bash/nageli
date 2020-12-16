using System;

namespace Nageli
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class TomlRenameAttribute : Attribute
    {
        public TomlRenameAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
