using System;

namespace Nageli.Converters
{
    internal sealed class NullableConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type)
            => type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public ITomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
            => (ITomlConverter)Activator.CreateInstance(
                typeof(NullableConverter<>).MakeGenericType(typeToConvert.GetGenericArguments()),
                options)!;
    }
}
