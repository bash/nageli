using System.Diagnostics.CodeAnalysis;

namespace Nageli
{
    /// <summary>Decides how absent keys are treated. See <see cref="TomlAbsentValuesPolicy"/> for some implementations.</summary>
    public interface ITomlAbsentValuesPolicy
    {
        [return: MaybeNull]
        T ConvertFromAbsent<T>(ITomlSerializerContext context);
    }
}
