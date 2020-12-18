using System;
using System.Reflection;

namespace Nageli.Features.NewType
{
    internal sealed record NewTypeMetadata(
        Type InnerType,
        TomlConverter InnerConverter,
        ConstructorInfo Constructor,
        PropertyInfo ValueProperty)
    {
    }
}
