namespace Nageli
{
    public static class TomlSerializerContextExtensions
    {
        public static ITomlConverter<T> GetConverter<T>(this ITomlSerializerContext context)
            => context.GetConverter(typeof(T)).AsTomlConverter<T>();
    }
}
