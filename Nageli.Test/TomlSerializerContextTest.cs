using System;
using Moq;
using Xunit;

namespace Nageli.Test
{
    public sealed class TomlSerializerContextTest
    {
        [Fact]
        public void DefaultImplementationCanBeResolved()
        {
            var context = TomlSerializerContext.Create(
                TomlSerializerOptions.Default
                    .AddDefaultImplementation<IInterface, DefaultImplementation>());
            var implementation = context.GetDefaultImplementation(typeof(IInterface));
            Assert.Equal(typeof(DefaultImplementation), implementation);
        }

        [Fact]
        public void DefaultImplementationCanBeResolvedThroughProvider()
        {
            var provider = Mock.Of<IDefaultImplementationProvider>(
                p => p.HasDefaultImplementation(typeof(string)) == true
                     && p.GetDefaultImplementation(typeof(string)) == typeof(int));

            var context = TomlSerializerContext.Create(
                TomlSerializerOptions.Default.AddDefaultImplementation(provider));
            var implementation = context.GetDefaultImplementation(typeof(string));
            Assert.Equal(typeof(int), implementation);
        }

        [Fact]
        public void NonSupportingDefaultImplementationProviderIsIgnored()
        {
            var nonSupportingProvider = Mock.Of<IDefaultImplementationProvider>(
                p => p.HasDefaultImplementation(It.IsAny<Type>()) == false);

            var context = TomlSerializerContext.Create(
                TomlSerializerOptions.Default
                    .AddDefaultImplementation<IInterface, DefaultImplementation>()
                    .AddDefaultImplementation(nonSupportingProvider));
            var implementation = context.GetDefaultImplementation(typeof(IInterface));
            Assert.Equal(typeof(DefaultImplementation), implementation);
        }

        [Fact]
        public void TheLatterDefaultImplementationTakesPrecedence()
        {
            var context = TomlSerializerContext.Create(
                TomlSerializerOptions.Default
                    .AddDefaultImplementation<IInterface, DefaultImplementation>()
                    .AddDefaultImplementation<IInterface, OtherDefaultImplementation>());
            var implementation = context.GetDefaultImplementation(typeof(IInterface));
            Assert.Equal(typeof(OtherDefaultImplementation), implementation);
        }

        [Fact]
        public void OpenGenericDefaultImplementationCanBeResolved()
        {
            var context = TomlSerializerContext.Create(
                TomlSerializerOptions.Default
                    .AddOpenGenericDefaultImplementation(
                        typeof(IGeneric<>),
                        @params => typeof(GenericImplementation<>).MakeGenericType(@params)));
            var implementation = context.GetDefaultImplementation(typeof(IGeneric<string>));
            Assert.Equal(typeof(GenericImplementation<string>), implementation);
        }

        private interface IInterface
        {
        }

        private sealed class DefaultImplementation : IInterface
        {
        }

        private sealed class OtherDefaultImplementation : IInterface
        {
        }

        private interface IGeneric<T>
        {
        }

        private sealed class GenericImplementation<T> : IGeneric<T>
        {
        }
    }
}
