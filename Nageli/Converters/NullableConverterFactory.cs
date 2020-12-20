using System;

namespace Nageli.Converters
{
    internal sealed class NullableConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type)
            => type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public ITomlConverter CreateConverter(Type typeToConvert, ITomlSerializerContext context)
            => (ITomlConverter)Activator.CreateInstance(
                typeof(NullableConverter<>).MakeGenericType(typeToConvert.GetGenericArguments()),
                context)!;
    }
}
