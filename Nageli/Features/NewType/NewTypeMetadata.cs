using System;
using System.Reflection;

namespace Nageli.Features.NewType
{
    internal sealed record NewTypeMetadata(
        Type InnerType,
        ITomlConverter InnerConverter,
        ConstructorInfo Constructor,
        PropertyInfo ValueProperty)
    {
    }
}
