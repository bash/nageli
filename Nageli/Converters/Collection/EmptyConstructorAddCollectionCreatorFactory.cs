using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nageli.Converters.Collection
{
    internal sealed class EmptyConstructorAddCollectionCreatorFactory : ICollectionCreatorFactory
    {
        public bool CanCreate(Type collectionType, Type itemType)
            => collectionType == typeof(ICollection<>).MakeGenericType(itemType)
               && collectionType.GetConstructors().Any(c => c.GetParameters().Length == 0);

        public CollectionCreator CreateCollectionCreator(Type collectionType, Type itemType)
            => typeof(EmptyConstructorAddCollectionCreatorFactory)
                .GetMethod(nameof(CreateCollection), BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(collectionType, itemType)
                .CreateDelegate<CollectionCreator>();

        private static object CreateCollection<TCollection, TItem>(IEnumerable items)
            where TCollection : ICollection<TItem>, new()
        {
            var collection = new TCollection();

            foreach (var item in items.Cast<TItem>())
            {
                collection.Add(item);
            }

            return collection;
        }
    }
}
