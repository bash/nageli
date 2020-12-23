using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using Nageli.Converters;
using Nageli.Converters.Collection;
using Nageli.DefaultImplementations;

namespace Nageli
{
    // TODO: make object deserialization strategy (constructor vs properties) configurable
    // TODO: Add a default naming policy that is used when the other policies are not specified
    public sealed record TomlSerializerOptions : ITomlToSerializerContext
    {
        public static TomlSerializerOptions Minimal { get; } = new(
            propertyNamingPolicy: TomlNamingPolicy.Default,
            absentValuesPolicy: TomlAbsentValuesPolicy.UseDefault,
            defaultImplementations: ImmutableArray<IDefaultImplementationProvider>.Empty,
            converters: ImmutableArray<ITomlConverterFactory>.Empty);

        public static TomlSerializerOptions Default { get; }
            = Minimal
                .AddConverters(ImmutableArray.Create<ITomlConverterFactory>(
                    new GenericConverterFactory<string>(new SimpleConverter<string>()),
                    new GenericConverterFactory<long>(new SimpleConverter<long>()),
                    new GenericConverterFactory<bool>(new SimpleConverter<bool>()),
                    new GenericConverterFactory<double>(new SimpleConverter<double>()),
                    new GenericConverterFactory<int>(new Int32Converter()),
                    new GenericConverterFactory<DateTime>(new SimpleConverter<DateTime>()),
                    new GenericConverterFactory<Guid>(new GuidConverter()),
                    new DictionaryConverterFactory(),
                    new NullableConverterFactory(),
                    new CollectionConverterFactory(),
                    new ObjectConverterFactory()))
                .AddOpenGenericDefaultImplementation(typeof(IEnumerable<>), typeof(List<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(ICollection<>), typeof(List<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IReadOnlyCollection<>), typeof(List<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IList<>), typeof(List<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IReadOnlyList<>), typeof(List<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(ISet<>), typeof(HashSet<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IReadOnlySet<>), typeof(HashSet<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IImmutableList<>), typeof(ImmutableList<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IImmutableQueue<>), typeof(ImmutableQueue<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IImmutableSet<>), typeof(ImmutableHashSet<>).MakeGenericType)
                .AddOpenGenericDefaultImplementation(typeof(IImmutableStack<>), typeof(ImmutableStack<>).MakeGenericType);

        /// <summary>Decides how absent keys are treated. See <see cref="TomlAbsentValuesPolicy"/> for some implementations.</summary>
        public ITomlAbsentValuesPolicy AbsentValuesPolicy { get; }

        public ITomlNamingPolicy PropertyNamingPolicy { get; }

        public IImmutableList<ITomlConverterFactory> Converters { get; }

        public IImmutableList<IDefaultImplementationProvider> DefaultImplementations { get; }

        private TomlSerializerOptions(
            ITomlAbsentValuesPolicy absentValuesPolicy,
            ITomlNamingPolicy propertyNamingPolicy,
            IImmutableList<ITomlConverterFactory> converters,
            IImmutableList<IDefaultImplementationProvider> defaultImplementations)
        {
            AbsentValuesPolicy = absentValuesPolicy;
            PropertyNamingPolicy = propertyNamingPolicy;
            Converters = converters;
            DefaultImplementations = defaultImplementations;
        }

        /// <summary>Decides how absent keys are treated. See <see cref="TomlAbsentValuesPolicy"/> for some implementations.</summary>
        [Pure]
        public TomlSerializerOptions WithAbsentValuesPolicy(ITomlAbsentValuesPolicy absentValuesPolicy)
            => ShallowClone(absentValuesPolicy: absentValuesPolicy);

        [Pure]
        public TomlSerializerOptions WithPropertyNamingPolicy(ITomlNamingPolicy namingPolicy)
            => ShallowClone(propertyNamingPolicy: namingPolicy);

        [Pure]
        public TomlSerializerOptions AddConverter(ITomlConverterFactory converterFactory)
            => WithConverters(ImmutableArray.Create(converterFactory).AddRange(Converters));

        [Pure]
        public TomlSerializerOptions AddConverters(IEnumerable<ITomlConverterFactory> converterFactories)
            => WithConverters(ImmutableArray.CreateRange(converterFactories).AddRange(Converters));

        [Pure]
        public TomlSerializerOptions AddConverter<T>(ITomlConverter<T> converter)
            => AddConverter(new GenericConverterFactory<T>(converter));

        [Pure]
        public TomlSerializerOptions WithConverters(IEnumerable<ITomlConverterFactory> converters)
            => ShallowClone(converters: converters.ToImmutableList());

        [Pure]
        public TomlSerializerOptions AddDefaultImplementation(IDefaultImplementationProvider defaultImplementation)
            => ShallowClone(defaultImplementations: ImmutableArray.Create(defaultImplementation).AddRange(DefaultImplementations));

        /// <param name="abstractType">An abstract type or an interface.</param>
        /// <param name="concreteType">An non-abstract, non-interface type that inherits from <paramref name="abstractType"/>.</param>
        /// <exception cref="NotSupportedException">Thrown when either of the types is a generic type definition (i.e. an open generic type).</exception>
        [Pure]
        public TomlSerializerOptions AddDefaultImplementation(Type abstractType, Type concreteType)
            => AddDefaultImplementation(new SimpleDefaultImplementationProvider(abstractType, concreteType));

        /// <typeparam name="TAbstract">An abstract type or an interface.</typeparam>
        /// <typeparam name="TImplementation">An non-abstract, non-interface type that inherits from <typeparamref name="TAbstract"/>.</typeparam>
        /// <exception cref="NotSupportedException">Thrown when either of the types is a generic type definition (i.e. an open generic type).</exception>
        [Pure]
        public TomlSerializerOptions AddDefaultImplementation<TAbstract, TImplementation>()
            where TImplementation : TAbstract
            => AddDefaultImplementation(typeof(TAbstract), typeof(TImplementation));

        [Pure]
        public TomlSerializerOptions AddOpenGenericDefaultImplementation(Type abstractType, Func<Type[], Type> createImplementation)
            => AddDefaultImplementation(new OpenGenericDefaultImplementationProvider(abstractType, createImplementation));

        private TomlSerializerOptions ShallowClone(
            ITomlAbsentValuesPolicy? absentValuesPolicy = null,
            ITomlNamingPolicy? propertyNamingPolicy = null,
            IImmutableList<ITomlConverterFactory>? converters = null,
            IImmutableList<IDefaultImplementationProvider>? defaultImplementations = null)
            => new(
                absentValuesPolicy: absentValuesPolicy ?? AbsentValuesPolicy,
                propertyNamingPolicy: propertyNamingPolicy ?? PropertyNamingPolicy,
                converters: converters ?? Converters,
                defaultImplementations: defaultImplementations ?? DefaultImplementations);

        public ITomlSerializerContext ToSerializerContext() => TomlSerializerContext.Create(this);
    }
}
