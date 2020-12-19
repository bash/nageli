using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Nageli.Features.NewType
{
    internal sealed class NewTypeConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type) => Attribute.IsDefined(type, typeof(TomlNewTypeAttribute));

        public ITomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
            => (ITomlConverter)Activator.CreateInstance(
                typeof(NewTypeConverter<>).MakeGenericType(typeToConvert),
                GetMetadata(typeToConvert, options))!;

        private static NewTypeMetadata GetMetadata(Type typeToConvert, TomlSerializerOptions options)
        {
            var constructor = FindConstructor(typeToConvert);
            var innerType = constructor.GetParameters()[0].ParameterType;
            var converter = options.GetConverter(innerType);
            return new NewTypeMetadata(innerType, converter, constructor, null!);
        }

        private static ConstructorInfo FindConstructor(Type typeToConvert)
        {
            var constructorCandidates = typeToConvert.GetConstructors().Where(IsUnary).ToImmutableArray();
            var markedConstructors = constructorCandidates.Where(IsMarkedAsTomlConstructor).ToImmutableArray();
            return (markedConstructors.Length, constructorCandidates.Length) switch
            {
                (0, 0) => throw new TomlException($"No suitable constructors found for type '{typeToConvert}'. There must be at least one constructor with one parameter"),
                (>1, _) => throw new TomlException($"Multiple constructors of type '{typeToConvert}' are marked with [TomlConstructor]"),
                (0, >1) => throw new TomlException($"Multiple suitable constructors found for type '{typeToConvert}'. Choose one with [TomlConstructor]"),
                (1, _) => markedConstructors[0],
                (_, 1) => constructorCandidates[0],
                _ => throw new InvalidOperationException(),
            };
        }

        private static bool IsMarkedAsTomlConstructor(ConstructorInfo constructor) => Attribute.IsDefined(constructor, typeof(TomlConstructorAttribute));

        private static bool IsUnary(MethodBase method) => method.GetParameters().Length == 1;
    }
}
