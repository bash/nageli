using System;
using Moq;
using Nageli;
using Xunit;

namespace Naegeli.Test
{
    public sealed class TomlSerializerOptionsTest
    {
        [Fact]
        public void DefaultImplementationCanBeResolved()
        {
            var options = TomlSerializerOptions.Default.AddDefaultImplementation<IInterface, DefaultImplementation>();
            var implementation = options.GetDefaultImplementation(typeof(IInterface));
            Assert.Equal(typeof(DefaultImplementation), implementation);
        }

        [Fact]
        public void DefaultImplementationCanBeResolvedThroughProvider()
        {
            var provider = Mock.Of<IDefaultImplementationProvider>(
                p => p.HasDefaultImplementation(typeof(string)) == true
                     && p.GetDefaultImplementation(typeof(string)) == typeof(int));

            var options = TomlSerializerOptions.Default.AddDefaultImplementation(provider);
            var implementation = options.GetDefaultImplementation(typeof(string));
            Assert.Equal(typeof(int), implementation);
        }

        [Fact]
        public void NonSupportingDefaultImplementationProviderIsIgnored()
        {
            var nonSupportingProvider = Mock.Of<IDefaultImplementationProvider>(
                p => p.HasDefaultImplementation(It.IsAny<Type>()) == false);

            var options = TomlSerializerOptions.Default
                .AddDefaultImplementation<IInterface, DefaultImplementation>()
                .AddDefaultImplementation(nonSupportingProvider);
            var implementation = options.GetDefaultImplementation(typeof(IInterface));
            Assert.Equal(typeof(DefaultImplementation), implementation);
        }

        [Fact]
        public void TheLatterDefaultImplementationTakesPrecedence()
        {
            var options = TomlSerializerOptions.Default
                .AddDefaultImplementation<IInterface, DefaultImplementation>()
                .AddDefaultImplementation<IInterface, OtherDefaultImplementation>();
            var implementation = options.GetDefaultImplementation(typeof(IInterface));
            Assert.Equal(typeof(OtherDefaultImplementation), implementation);
        }

        [Fact]
        public void OpenGenericDefaultImplementationCanBeResolved()
        {
            var options = TomlSerializerOptions.Default
                .AddOpenGenericDefaultImplementation(
                    typeof(IGeneric<>),
                    @params => typeof(GenericImplementation<>).MakeGenericType(@params));
            var implementation = options.GetDefaultImplementation(typeof(IGeneric<string>));
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
