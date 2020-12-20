using System.Diagnostics.CodeAnalysis;

namespace Nageli.AbsentValuesPolicies
{
    internal sealed class UseDefaultAbsentValuesPolicy : ITomlAbsentValuesPolicy
    {
        [return: MaybeNull]
        public T ConvertFromAbsent<T>(ITomlSerializerContext context) => default;
    }
}
