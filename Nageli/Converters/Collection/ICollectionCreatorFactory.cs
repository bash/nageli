using System;
using System.Collections;
using System.Collections.Generic;

// TODO: move these public types to a better namespace
namespace Nageli.Converters.Collection
{
    /// <remarks>Note that <paramref name="items"/> is guaranteed to be an <see cref="IEnumerable{T}"/>.</remarks>
    public delegate object CollectionCreator(IEnumerable items);

    public interface ICollectionCreatorFactory
    {
        bool CanCreate(Type collectionType, Type itemType);

        CollectionCreator CreateCollectionCreator(Type collectionType, Type itemType);
    }
}
