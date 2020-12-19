using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using Nageli.Converters;

namespace Nageli
{
    // TODO: make object deserialization strategy (constructor vs properties) configurable
    // TODO: Add a default naming policy that is used when the other policies are not specified
    public sealed record TomlSerializerOptions
    {
        public static TomlSerializerOptions Default { get; } = new(
            propertyNamingPolicy: TomlNamingPolicy.Default,
            absentValuesPolicy: AbsentValuesPolicy.UseDefault,
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

        private readonly IDictionary<Type, ITomlConverter> _cachedConverters = new ConcurrentDictionary<Type, ITomlConverter>();

        public AbsentValuesPolicy AbsentValuesPolicy { get; }

        public ITomlNamingPolicy PropertyNamingPolicy { get; }

        public IImmutableList<ITomlConverterFactory> Converters { get; }

        private TomlSerializerOptions(
            AbsentValuesPolicy absentValuesPolicy,
            ITomlNamingPolicy propertyNamingPolicy,
            IImmutableList<ITomlConverterFactory> converters)
        {
            AbsentValuesPolicy = absentValuesPolicy;
            PropertyNamingPolicy = propertyNamingPolicy;
            Converters = converters;
        }

        [Pure]
        public TomlSerializerOptions WithAbsentValuesPolicy(AbsentValuesPolicy absentValuesPolicy)
            => ShallowClone(absentValuesPolicy: absentValuesPolicy);

        [Pure]
        public TomlSerializerOptions WithPropertyNamingPolicy(ITomlNamingPolicy namingPolicy)
            => ShallowClone(propertyNamingPolicy: namingPolicy);

        [Pure]
        public TomlSerializerOptions AddConverter(ITomlConverterFactory converterFactory)
            => WithConverters(ImmutableArray.Create(converterFactory).AddRange(Converters));

        [Pure]
        public TomlSerializerOptions AddConverter<T>(ITomlConverter<T> converter)
            where T : notnull
            => AddConverter(new GenericConverterFactory<T>(converter));

        [Pure]
        public TomlSerializerOptions WithConverters(IEnumerable<ITomlConverterFactory> converters)
            => ShallowClone(converters: converters.ToImmutableList());

        [Pure]
        public ITomlConverter GetConverter(Type typeToConvert)
        {
            if (_cachedConverters.TryGetValue(typeToConvert, out var cachedConverter))
            {
                return cachedConverter;
            }

            var factory = Converters.First(c => c.CanConvert(typeToConvert));
            var converter = factory.CreateConverter(typeToConvert, this);
            _cachedConverters.TryAdd(typeToConvert, converter);
            return converter;
        }

        [Pure]
        public ITomlConverter<T> GetConverter<T>()
            where T : notnull
            => GetConverter(typeof(T)).AsTomlConverter<T>();

        private TomlSerializerOptions ShallowClone(
            AbsentValuesPolicy? absentValuesPolicy = null,
            ITomlNamingPolicy? propertyNamingPolicy = null,
            IImmutableList<ITomlConverterFactory>? converters = null)
            => new(
                absentValuesPolicy: absentValuesPolicy ?? AbsentValuesPolicy,
                propertyNamingPolicy: propertyNamingPolicy ?? PropertyNamingPolicy,
                converters: converters ?? Converters);
    }
}
