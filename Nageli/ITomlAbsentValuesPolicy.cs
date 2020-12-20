using System.Diagnostics.CodeAnalysis;

namespace Nageli
{
    public interface ITomlAbsentValuesPolicy
    {
        [return: MaybeNull]
        T ConvertFromAbsent<T>(ITomlSerializerContext context);
    }
}
