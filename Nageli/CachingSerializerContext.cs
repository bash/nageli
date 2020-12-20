using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Nageli
{
    internal sealed class CachingSerializerContext : ITomlSerializerContext
    {
        private readonly ConcurrentDictionary<Type, ITomlConverter> _cachedConverters = new();

        private readonly ConcurrentDictionary<Type, Type?> _cachedDefaultImplementations = new();

        public CachingSerializerContext(TomlSerializerOptions options) => Options = options;

        public TomlSerializerOptions Options { get; }

        public ITomlConverter GetConverter(Type typeToConvert)
            => _cachedConverters.GetOrAdd(typeToConvert, GetConverterUncached);

        public Type? GetDefaultImplementation(Type typeToConvert)
            => _cachedDefaultImplementations.GetOrAdd(typeToConvert, GetImplementationUncached);

        private ITomlConverter GetConverterUncached(Type typeToConvert)
        {
            var factory = Options.Converters.FirstOrDefault(c => c.CanConvert(typeToConvert))
                ?? throw new TomlException($"No matching converter found for type '{typeToConvert}'");
            return factory.CreateConverter(typeToConvert, this);
        }

        private Type? GetImplementationUncached(Type typeToConvert)
        {
            var provider = Options.DefaultImplementations.FirstOrDefault(c => c.HasDefaultImplementation(typeToConvert));
            return provider?.GetDefaultImplementation(typeToConvert);
        }
    }
}
