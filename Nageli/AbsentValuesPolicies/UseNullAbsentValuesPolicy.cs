using System.Diagnostics.CodeAnalysis;

namespace Nageli.AbsentValuesPolicies
{
    internal sealed class UseNullAbsentValuesPolicy : ITomlAbsentValuesPolicy
    {
        [return: MaybeNull]
        public T ConvertFromAbsent<T>(ITomlSerializerContext context)
            => typeof(T).IsValueType
                ? throw new TomlException()
                : default;
    }
}
