using System;
using Funcky.Monads;

namespace Nageli.Features.Option
{
    internal sealed class OptionConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type)
            => type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Option<>);

        public ITomlConverter CreateConverter(Type optionType, ITomlSerializerContext context)
            => (ITomlConverter)Activator.CreateInstance(
                typeof(OptionConverter<>).MakeGenericType(optionType.GetGenericArguments()),
                context)!;
    }
}
