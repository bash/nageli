using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Nageli.Converters
{
    public sealed class ObjectConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type) => true;

        public TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
        {
            var constructor = GetConstructorForDeserialization(typeToConvert);
            var parameterConverters = constructor.GetParameters().Select(p => GetCachedParameterInfo(p, options));
            return (TomlConverter)Activator.CreateInstance(
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
        {
            var constructors = typeToConvert.GetConstructors();
            return constructors.SingleOrDefault(c => Attribute.IsDefined(c, typeof(TomlConstructorAttribute)))
                ?? constructors.FirstOrDefault()
                ?? throw new TomlException($"No public constructor found for {typeToConvert}");
        }
    }
}
