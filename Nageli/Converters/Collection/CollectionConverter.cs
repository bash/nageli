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
            TomlSerializerOptions options)
        {
            _createCollection = createCollection;
            _itemConverter = options.GetConverter<TItem>();
        }

        public TCollection ConvertFrom(TomlObject value, TomlSerializerOptions options)
        {
            TCollection CreateCollection(IEnumerable<TomlObject> values)
                => (TCollection)_createCollection(values.Select(v => _itemConverter.ConvertFrom(v, options)));

            return value switch
            {
                TomlTableArray tableArray => CreateCollection(tableArray),
                TomlArray tomlArray => CreateCollection(tomlArray.GetTomlEnumerator()),
                _ => throw new TomlException(),
            };
        }

        public object ConvertFromAbsent(TomlSerializerOptions options)
            => _createCollection(Enumerable.Empty<TItem>());

        public TomlObject ConvertTo(TCollection value, TomlSerializerOptions options) => throw new NotImplementedException();
    }
}
