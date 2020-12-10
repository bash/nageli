using System;
using Funcky.Monads;
using Nageli;

namespace Funcky.Nageli
{
    public sealed class OptionConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type)
            => type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Option<>);

        public TomlConverter CreateConverter(Type optionType, TomlSerializerOptions options)
            => (TomlConverter)Activator.CreateInstance(
                typeof(OptionConverter<>).MakeGenericType(optionType.GetGenericArguments()),
                options)!;
    }
}
