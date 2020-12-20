using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nageli.Converters.Collection
{
    internal sealed class ConstructorBasedCollectionCreatorFactory : ICollectionCreatorFactory
    {
        public bool CanCreate(Type collectionType, Type itemType)
            => FindConstructor(collectionType, itemType) is not null;

        public CollectionCreator CreateCollectionCreator(Type collectionType, Type itemType)
        {
            var constructor = FindConstructor(collectionType, itemType)!;
            return items => constructor.Invoke(new object?[] { items });
        }

        private static ConstructorInfo? FindConstructor(Type collectionType, Type itemType)
            => collectionType
                .GetConstructors()
                .Where(c => c.GetParameters().Length == 1)
                .FirstOrDefault(c => c.GetParameters()[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(itemType));
    }
}
