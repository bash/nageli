using System.Reflection;

namespace Nageli.Converters
{
    internal record CachedParameterInfo(ParameterInfo Info, TomlConverter Converter)
    {
    }
}
