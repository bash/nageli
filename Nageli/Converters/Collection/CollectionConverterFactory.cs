using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Nageli.Converters.Collection
{
    internal sealed class CollectionConverterFactory : ITomlConverterFactory
    {
        private static readonly IImmutableList<ICollectionCreatorFactory> CollectionCreatorFactories
            = ImmutableArray.Create<ICollectionCreatorFactory>(
                new ConstructorBasedCollectionCreatorFactory(),
                new EmptyConstructorAddCollectionCreatorFactory(),
                new ImmutableCollectionCreatorFactory());

        public bool CanConvert(Type type) => IsIEnumerable(type) || type.GetInterfaces().Any(IsIEnumerable);

        public ITomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
        {
            var itemType = IsIEnumerable(typeToConvert)
                ? typeToConvert.GetGenericArguments()[0]
                : typeToConvert.GetInterfaces().Single(IsIEnumerable).GetGenericArguments()[0];

            var typeToCreate = options.GetDefaultImplementation(typeToConvert) ?? typeToConvert;

            var factory = CollectionCreatorFactories.FirstOrDefault(f => f.CanCreate(typeToCreate, itemType))
                ?? throw new TomlException($"No creator found for collection {typeToCreate}");

            return (ITomlConverter)Activator.CreateInstance(
                typeof(CollectionConverter<,>).MakeGenericType(typeToConvert, itemType),
                factory.CreateCollectionCreator(typeToCreate, itemType),
                options)!;
        }

        private static bool IsIEnumerable(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);

        private static bool IsISet(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ISet<>);

        private static bool IsReadonlyCollection(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>);

        private static bool IsReadonlySet(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReadOnlySet<>);
    }
}
