using System;
using Funcky.Monads;
using Nageli;
using Tomlyn.Model;

namespace Funcky.Nageli
{
    public sealed class OptionConverter<TItem> : TomlConverter<Option<TItem>>
        where TItem : notnull
    {
        private readonly TomlConverter<TItem> _itemConverter;

        public OptionConverter(TomlSerializerOptions options) => _itemConverter = options.GetConverter<TItem>();

        public override Option<TItem> ConvertFrom(TomlObject value, TomlSerializerOptions options) => _itemConverter.ConvertFrom(value, options);

        public override Option<TItem> ConvertFromAbsent(TomlSerializerOptions options) => Option<TItem>.None();

        public override TomlObject ConvertTo(Option<TItem> value, TomlSerializerOptions options) => throw new System.NotImplementedException();
    }

    public sealed class OptionConverterFactory : TomlConverterFactory
    {
        public override bool CanConvert(Type type)
            => type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Option<>);

        public override TomlConverter CreateConverter(Type optionType, TomlSerializerOptions options)
            => (TomlConverter)Activator.CreateInstance(
                typeof(OptionConverter<>).MakeGenericType(optionType.GetGenericArguments()),
                options)!;
    }
}
