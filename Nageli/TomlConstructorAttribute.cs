using System;

namespace Nageli
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class TomlConstructorAttribute : Attribute
    {
    }
}
