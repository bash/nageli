namespace Nageli
{
    public static class TomlSerializerContext
    {
        public static ITomlSerializerContext Create(TomlSerializerOptions? options = null)
            => new CachingSerializerContext(options ?? TomlSerializerOptions.Default);
    }
}
