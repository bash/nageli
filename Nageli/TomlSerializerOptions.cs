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
            converters: ImmutableArray.Create<ITomlConverterFactory>(
                new GenericConverterFactory<string>(new SimpleConverter<string>()),
                new GenericConverterFactory<long>(new SimpleConverter<long>()),
                new GenericConverterFactory<bool>(new SimpleConverter<bool>()),
                new GenericConverterFactory<double>(new SimpleConverter<double>()),
                new GenericConverterFactory<int>(new Int32Converter()),
                new GenericConverterFactory<DateTime>(new SimpleConverter<DateTime>()),
                new GenericConverterFactory<Guid>(new GuidConverter()),
                new DictionaryConverterFactory(),
                new NullableConverterFactory(),
                new ObjectConverterFactory()));

        private readonly IDictionary<Type, TomlConverter> _cachedConverters = new ConcurrentDictionary<Type, TomlConverter>();

        public ITomlNamingPolicy PropertyNamingPolicy { get; }

        public IImmutableList<ITomlConverterFactory> Converters { get; }

        private TomlSerializerOptions(ITomlNamingPolicy propertyNamingPolicy, IImmutableList<ITomlConverterFactory> converters)
        {
            PropertyNamingPolicy = propertyNamingPolicy;
            Converters = converters;
        }

        [Pure]
        public TomlSerializerOptions WithPropertyNamingPolicy(ITomlNamingPolicy namingPolicy)
            => ShallowClone(propertyNamingPolicy: namingPolicy);

        [Pure]
        public TomlSerializerOptions WithConverter(ITomlConverterFactory converterFactory)
            => ShallowClone(converters: ImmutableArray.Create(converterFactory).AddRange(Converters));

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
            IImmutableList<ITomlConverterFactory>? converters = null)
            => new(
                propertyNamingPolicy: propertyNamingPolicy ?? PropertyNamingPolicy,
                converters: converters ?? Converters);
    }
}
