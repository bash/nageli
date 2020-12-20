using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Nageli.Converters.Collection
{
    internal sealed class ImmutableCollectionCreatorFactory : ICollectionCreatorFactory
    {
        public bool CanCreate(Type collectionType, Type itemType)
        {
            return collectionType.IsGenericType
                   && collectionType.GetGenericTypeDefinition() is var genericTypeDefinition
                   && (genericTypeDefinition == typeof(ImmutableList<>)
                       || genericTypeDefinition == typeof(ImmutableArray<>)
                       || genericTypeDefinition == typeof(ImmutableQueue<>)
                       || genericTypeDefinition == typeof(ImmutableStack<>)
                       || genericTypeDefinition == typeof(ImmutableHashSet<>)
                       || genericTypeDefinition == typeof(ImmutableSortedSet<>));
        }

        public CollectionCreator CreateCollectionCreator(Type collectionType, Type itemType)
            => collectionType.GetGenericTypeDefinition() switch
            {
                var type when type == typeof(ImmutableList<>)
                    => CreateImmutableCollectionCreator(itemType, typeof(ImmutableList), nameof(ImmutableList.CreateRange)),
                var type when type == typeof(ImmutableArray<>)
                    => CreateImmutableCollectionCreator(itemType, typeof(ImmutableArray), nameof(ImmutableArray.CreateRange)),
                var type when type == typeof(ImmutableQueue<>)
                    => CreateImmutableCollectionCreator(itemType, typeof(ImmutableQueue), nameof(ImmutableQueue.CreateRange)),
                var type when type == typeof(ImmutableStack<>)
                    => CreateImmutableCollectionCreator(itemType, typeof(ImmutableStack), nameof(ImmutableStack.CreateRange)),
                var type when type == typeof(ImmutableHashSet<>)
                    => CreateImmutableCollectionCreator(itemType, typeof(ImmutableHashSet), nameof(ImmutableHashSet.CreateRange)),
                var type when type == typeof(ImmutableSortedSet<>)
                    => CreateImmutableCollectionCreator(itemType, typeof(ImmutableSortedSet), nameof(ImmutableSortedSet.CreateRange)),
                _ => throw new NotSupportedException(),
            };

        private static CollectionCreator CreateImmutableCollectionCreator(Type itemType, Type type, string methodName)
            => (CollectionCreator)typeof(ImmutableCollectionCreatorFactory)
                .GetMethod(nameof(CreateImmutableCollectionCreatorInternal), BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(itemType)
                .Invoke(null, new object?[] { type, methodName })!;

        private static CollectionCreator CreateImmutableCollectionCreatorInternal<TItem>(Type type, string methodName)
        {
            var createCollectionGeneric = type
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Single(m => m.Name == methodName && IsImmutableCollectionCreationMethod(m))
                .MakeGenericMethod(typeof(TItem))
                .CreateDelegate<Func<IEnumerable<TItem>, object>>();
            return items => createCollectionGeneric((IEnumerable<TItem>)items);
        }

        private static bool IsImmutableCollectionCreationMethod(MethodInfo method)
            => method.IsGenericMethod
                && method.GetGenericArguments().Length == 1
                && method.GetParameters().Length == 1
                && method.GetParameters()[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(method.GetGenericArguments());
    }
}
