using System;
using System.Collections.Generic;
using System.Linq;
using Tomlyn.Model;

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
        {
            TCollection CreateCollection(IEnumerable<TomlObject> values)
                => (TCollection)_createCollection(values.Select(v => _itemConverter.ConvertFrom(v, context)));

            return value switch
            {
                TomlTableArray tableArray => CreateCollection(tableArray),
                TomlArray tomlArray => CreateCollection(tomlArray.GetTomlEnumerator()),
                _ => throw new TomlException(),
            };
        }

        public TCollection ConvertFromAbsent(ITomlSerializerContext context)
            => (TCollection)_createCollection(Enumerable.Empty<TItem>());

        public TomlObject ConvertTo(TCollection value, ITomlSerializerContext context) => throw new NotImplementedException();
    }
}
