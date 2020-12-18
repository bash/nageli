using System;

namespace Nageli.Features.NewType
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class TomlNewTypeAttribute : Attribute
    {
    }
}
