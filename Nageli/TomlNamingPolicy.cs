namespace Nageli
{
    public static class TomlNamingPolicy
    {
        public static ITomlNamingPolicy Default { get; } = new UpperCamelCaseNamingPolicy();

        public static ITomlNamingPolicy SnakeCase { get; } = new SnakeCaseNamingPolicy();
    }
}
