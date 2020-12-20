namespace Nageli.AbsentValuesPolicies
{
    internal sealed class ThrowingAbsentValuesPolicy : ITomlAbsentValuesPolicy
    {
        public T ConvertFromAbsent<T>(ITomlSerializerContext context) => throw new TomlException();
    }
}
