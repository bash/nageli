using System;
using System.Collections.Generic;
using System.Linq;
using Nageli.Model;

namespace Nageli.Converters.Collection
{
    internal class CollectionConverter<TCollection, TItem> : ITomlConverter<TCollection>
    {
        private readonly CollectionCreator _createCollection;
        private readonly ITomlConverter<TItem> _itemConverter;

        public CollectionConverter(
            CollectionCreator createCollection,
            ITomlSerializerContext context)
        {
            _createCollection = createCollection;
            _itemConverter = context.GetConverter<TItem>();
        }

        public TCollection ConvertFrom(TomlObject value, ITomlSerializerContext context)
            => value is TomlArray tomlArray
                ? (TCollection)_createCollection(tomlArray.Select(v => _itemConverter.ConvertFrom(v, context)))
                : throw new TomlException();

        public TCollection ConvertFromAbsent(ITomlSerializerContext context)
            => (TCollection)_createCollection(Enumerable.Empty<TItem>());

        public TomlObject ConvertTo(TCollection value, ITomlSerializerContext context) => throw new NotImplementedException();
    }
}
