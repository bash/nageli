using Funcky.Monads;
using Tomlyn.Model;

namespace Nageli.Features.Option
{
    internal sealed class OptionConverter<TItem> : ITomlConverter<Option<TItem>>
        where TItem : notnull
    {
        private readonly ITomlConverter<TItem> _itemConverter;

        public OptionConverter(TomlSerializerOptions options) => _itemConverter = options.GetConverter<TItem>();

        public Option<TItem> ConvertFrom(TomlObject value, TomlSerializerOptions options) => _itemConverter.ConvertFrom(value, options);

        public Option<TItem> ConvertFromAbsent(TomlSerializerOptions options) => Option<TItem>.None();

        public TomlObject ConvertTo(Option<TItem> value, TomlSerializerOptions options) => throw new System.NotImplementedException();
    }
}
