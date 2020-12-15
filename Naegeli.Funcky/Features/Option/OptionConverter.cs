using Funcky.Monads;
using Tomlyn.Model;

namespace Nageli.Features.Option
{
    internal sealed class OptionConverter<TItem> : TomlConverter<Option<TItem>>
        where TItem : notnull
    {
        private readonly TomlConverter<TItem> _itemConverter;

        public OptionConverter(TomlSerializerOptions options) => _itemConverter = options.GetConverter<TItem>();

        public override Option<TItem> ConvertFrom(TomlObject value, TomlSerializerOptions options) => _itemConverter.ConvertFrom(value, options);

        public override Option<TItem> ConvertFromAbsent(TomlSerializerOptions options) => Option<TItem>.None();

        public override TomlObject ConvertTo(Option<TItem> value, TomlSerializerOptions options) => throw new System.NotImplementedException();
    }
}
