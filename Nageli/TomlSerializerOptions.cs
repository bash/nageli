using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using Nageli.Converters;

namespace Nageli
{
    public sealed record TomlSerializerOptions
    {
        public static TomlSerializerOptions Default { get; } = new(
            propertyNamingPolicy: TomlNamingPolicy.Default,
            converters: ImmutableArray.Create<TomlConverterFactory>(
                new SimpleConverter<string>(),
                new SimpleConverter<long>(),
                new SimpleConverter<bool>(),
                new SimpleConverter<double>(),
                new Int32Converter(),
                new SimpleConverter<DateTime>(),
                new GuidConverter(),
                new TomlObjectConverter(),
                new DictionaryConverterFactory(),
                new NullableConverterFactory(),
                new ObjectConverterFactory()));

        private readonly IDictionary<Type, TomlConverter> _cachedConverters = new ConcurrentDictionary<Type, TomlConverter>();

        public ITomlNamingPolicy PropertyNamingPolicy { get; }

        public IImmutableList<TomlConverterFactory> Converters { get; }

        private TomlSerializerOptions(ITomlNamingPolicy propertyNamingPolicy, IImmutableList<TomlConverterFactory> converters)
        {
            PropertyNamingPolicy = propertyNamingPolicy;
            Converters = converters;
        }

        [Pure]
        public TomlSerializerOptions WithPropertyNamingPolicy(ITomlNamingPolicy namingPolicy)
            => ShallowClone(propertyNamingPolicy: namingPolicy);

        [Pure]
        public TomlSerializerOptions WithConverters(params TomlConverterFactory[] converters)
            => ShallowClone(converters: ImmutableArray.Create(converters).AddRange(Converters));

        [Pure]
        public TomlConverter GetConverter(Type typeToConvert)
        {
            if (_cachedConverters.TryGetValue(typeToConvert, out var cachedConverter))
            {
                return cachedConverter;
            }

            var converter = Converters.First(c => c.CanConvert(typeToConvert))
                .CreateConverter(typeToConvert, this);
            _cachedConverters.TryAdd(typeToConvert, converter);
            return converter;
        }

        [Pure]
        public TomlConverter<T> GetConverter<T>()
            where T : notnull
            => (TomlConverter<T>)GetConverter(typeof(T));

        private TomlSerializerOptions ShallowClone(
            ITomlNamingPolicy? propertyNamingPolicy = null,
            IImmutableList<TomlConverterFactory>? converters = null)
            => new(
                propertyNamingPolicy: propertyNamingPolicy ?? PropertyNamingPolicy,
                converters: converters ?? Converters);
    }
}
