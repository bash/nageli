using Nageli.NamingPolicies;

namespace Nageli
{
    public static class TomlNamingPolicy
    {
        public static ITomlNamingPolicy Default { get; } = new UpperCamelCaseNamingPolicy();

        public static ITomlNamingPolicy SnakeCase { get; } = new SnakeCaseNamingPolicy();

        public static ITomlNamingPolicy CamelCase { get; } = new CamelCaseNamingPolicy();
    }
}
