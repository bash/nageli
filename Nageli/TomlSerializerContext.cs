namespace Nageli
{
    public static class TomlSerializerContext
    {
        public static ITomlSerializerContext Create(TomlSerializerOptions options) => new CachingSerializerContext(options);
    }
}
