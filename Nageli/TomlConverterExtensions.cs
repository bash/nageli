namespace Nageli
{
    public static class TomlConverterExtensions
    {
        public static ITomlConverter AsTomlConverter<T>(this ITomlConverter<T> converter) => converter;

        public static ITomlConverter<T> AsTomlConverter<T>(this ITomlConverter converter) => (ITomlConverter<T>)converter;
    }
}
