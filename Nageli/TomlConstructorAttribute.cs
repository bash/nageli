using System;

namespace Nageli
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TomlConstructorAttribute : Attribute
    {
    }
}
