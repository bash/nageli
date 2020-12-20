using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nageli
{
    internal sealed class TomlSerializerContext : ITomlSerializerContext
    {
        private readonly IDictionary<Type, ITomlConverter> _cachedConverters = new ConcurrentDictionary<Type, ITomlConverter>();

        private readonly IDictionary<Type, Type?> _cachedDefaultImplementations = new ConcurrentDictionary<Type, Type?>();

        public TomlSerializerContext(TomlSerializerOptions options)
        {
            Options = options;
        }

        public TomlSerializerOptions Options { get; }

        public ITomlConverter GetConverter(Type typeToConvert)
        {
            if (_cachedConverters.TryGetValue(typeToConvert, out var cachedConverter))
            {
                return cachedConverter;
            }

            var factory = Options.Converters.First(c => c.CanConvert(typeToConvert));
            var converter = factory.CreateConverter(typeToConvert, Options);
            _cachedConverters.TryAdd(typeToConvert, converter);
            return converter;
        }

        public Type? GetDefaultImplementation(Type typeToConvert)
        {
            if (_cachedDefaultImplementations.TryGetValue(typeToConvert, out var cachedImplementation))
            {
                return cachedImplementation;
            }

            var provider = Options.DefaultImplementations.FirstOrDefault(c => c.HasDefaultImplementation(typeToConvert));
            var implementation = provider?.GetDefaultImplementation(typeToConvert);
            _cachedDefaultImplementations.TryAdd(typeToConvert, implementation);
            return implementation;
        }
    }
}
