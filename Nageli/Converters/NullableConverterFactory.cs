using System;

namespace Nageli.Converters
{
    internal sealed class NullableConverterFactory : TomlConverterFactory
    {
        public override bool CanConvert(Type type)
            => type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public override TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
            => (TomlConverter)Activator.CreateInstance(
                typeof(NullableConverter<>).MakeGenericType(typeToConvert.GetGenericArguments()),
                options)!;
    }
}
