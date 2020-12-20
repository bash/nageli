using Nageli.AbsentValuesPolicies;

namespace Nageli
{
    public static class TomlAbsentValuesPolicy
    {
        /// <summary>
        /// Uses the default value for the type (<c>null</c> for reference types).
        /// </summary>
        public static ITomlAbsentValuesPolicy UseDefault { get; } = new UseDefaultAbsentValuesPolicy();

        /// <summary>
        /// Uses <c>null</c> for reference types and throws for nullable value types.
        /// </summary>
        public static ITomlAbsentValuesPolicy UseNull { get; } = new UseNullAbsentValuesPolicy();

        /// <summary>
        /// Throws when values are missing.
        /// </summary>
        public static ITomlAbsentValuesPolicy Disallow { get; } = new ThrowingAbsentValuesPolicy();
    }
}
