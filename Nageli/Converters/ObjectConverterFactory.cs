using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Nageli.Converters
{
    public sealed class ObjectConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type) => true;

        public ITomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
        {
            var constructor = GetConstructorForDeserialization(typeToConvert);
            var parameterConverters = constructor.GetParameters().Select(p => GetCachedParameterInfo(p, options));
            return (ITomlConverter)Activator.CreateInstance(
                typeof(ObjectConverter<>).MakeGenericType(typeToConvert),
                constructor,
                parameterConverters.ToImmutableList())!;
        }

        private static CachedParameterInfo GetCachedParameterInfo(ParameterInfo info, TomlSerializerOptions options)
        {
            var converter = options.GetConverter(info.ParameterType);
            return new CachedParameterInfo(info, converter);
        }

        private static ConstructorInfo GetConstructorForDeserialization(Type typeToConvert)
            => GetMarkedConstructor(typeToConvert)
                ?? GetFirstConstructorWithBiggestArity(typeToConvert)
                ?? throw new TomlException($"No public constructor found for type {typeToConvert}");

        private static ConstructorInfo? GetFirstConstructorWithBiggestArity(Type typeToConvert)
            => typeToConvert
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

        private static ConstructorInfo? GetMarkedConstructor(Type typeToConvert)
        {
            var markedConstructors = typeToConvert
                .GetConstructors()
                .Where(c => Attribute.IsDefined(c, typeof(TomlConstructorAttribute)))
                .ToImmutableList();
            return markedConstructors.Count switch
            {
                0 => null,
                1 => markedConstructors[0],
                _ => throw new TomlException($"Multiple constructors marked with [TomlConstructor] found for type {typeToConvert}"),
            };
        }
    }
}
