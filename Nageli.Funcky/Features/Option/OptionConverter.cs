using Funcky.Monads;
using Tomlyn.Model;

namespace Nageli.Features.Option
{
    internal sealed class OptionConverter<TItem> : ITomlConverter<Option<TItem>>
        where TItem : notnull
    {
        private readonly ITomlConverter<TItem> _itemConverter;

        public OptionConverter(ITomlSerializerContext context) => _itemConverter = context.GetConverter<TItem>();

        public Option<TItem> ConvertFrom(TomlObject value, ITomlSerializerContext context) => _itemConverter.ConvertFrom(value, context);

        public Option<TItem> ConvertFromAbsent(ITomlSerializerContext context) => Option<TItem>.None();

        public TomlObject ConvertTo(Option<TItem> value, ITomlSerializerContext context) => throw new System.NotImplementedException();
    }
}
