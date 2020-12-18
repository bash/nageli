using System;
using Nageli;
using Nageli.Features.NewType;
using Tomlyn.Model;
using Xunit;

namespace Naegeli.Test.Features.NewType
{
    public sealed class NewTypeConverterTest
    {
        // TODO: Add tests with valid and invalid properties

        [Theory]
        [InlineData(typeof(NewTypeWithNoSuitableConstructors))]
        [InlineData(typeof(NewTypeWithMoreThanOneSuitableConstructor))]
        [InlineData(typeof(NewTypeWithMoreThanOneMarkedConstructors))]
        [InlineData(typeof(NewTypeWithUnsuitableMarkedConstructor))]
        public void CreatingTheConverterForNewTypeWithNoSuitableConstructorThrows(Type typeToConvert)
        {
            var options = TomlSerializerOptions.Default.AddNewTypeConverter();
            Assert.Throws<TomlException>(() => options.GetConverter(typeToConvert));
        }

        [Theory]
        [MemberData(nameof(DeserializesNewTypesWithSuitableConstructorsData))]
        public void DeserializesNewTypesWithSuitableConstructors(TomlObject input, object expectedValue)
        {
            var options = TomlSerializerOptions.Default.AddNewTypeConverter();
            var value = TomlSerializer.Deserialize(input, expectedValue.GetType(), options);
            Assert.Equal(expectedValue, value);
        }

        public static TheoryData<TomlObject, object> DeserializesNewTypesWithSuitableConstructorsData()
            => new()
            {
                {
                    new TomlString("foo bar"),
                    new NewTypeWithOneSuitableConstructor("foo bar")
                },
                {
                    new TomlString("foo bar"),
                    new NewTypeWithOneSuitableConstructorAndNonSuitableConstructors("foo bar")
                },
                {
                    new TomlString("foo bar"),
                    new NewTypeWithMultipleSuitableConstructorsAndOneMarked("foo bar")
                },
            };

        [TomlNewType]
        private sealed class NewTypeWithNoSuitableConstructors
        {
            public NewTypeWithNoSuitableConstructors()
            {
            }

            public NewTypeWithNoSuitableConstructors(string foo, string bar)
            {
            }

            public NewTypeWithNoSuitableConstructors(string foo, string bar, string baz)
            {
            }
        }

        [TomlNewType]
        private sealed record NewTypeWithOneSuitableConstructor
        {
            public string Value { get; }

            public NewTypeWithOneSuitableConstructor(string value)
            {
                Value = value;
            }
        }

        [TomlNewType]
        private sealed record NewTypeWithOneSuitableConstructorAndNonSuitableConstructors
        {
            public string Value { get; }

            public NewTypeWithOneSuitableConstructorAndNonSuitableConstructors(string value)
            {
                Value = value;
            }

            public NewTypeWithOneSuitableConstructorAndNonSuitableConstructors(string value, string bar, string baz)
            {
                Value = value;
            }
        }

        [TomlNewType]
        private sealed record NewTypeWithMultipleSuitableConstructorsAndOneMarked
        {
            public string Value { get; }

            [TomlConstructor]
            public NewTypeWithMultipleSuitableConstructorsAndOneMarked(string value)
            {
                Value = value;
            }

            public NewTypeWithMultipleSuitableConstructorsAndOneMarked(int bar, string value)
            {
                Value = value;
            }
        }

        [TomlNewType]
        private sealed class NewTypeWithMoreThanOneSuitableConstructor
        {
            public NewTypeWithMoreThanOneSuitableConstructor(string foo)
            {
            }

            public NewTypeWithMoreThanOneSuitableConstructor(int bar)
            {
            }
        }

        [TomlNewType]
        private sealed class NewTypeWithMoreThanOneMarkedConstructors
        {
            [TomlConstructor]
            public NewTypeWithMoreThanOneMarkedConstructors(string foo)
            {
            }

            [TomlConstructor]
            public NewTypeWithMoreThanOneMarkedConstructors(int bar)
            {
            }
        }

        [TomlNewType]
        private sealed class NewTypeWithUnsuitableMarkedConstructor
        {
            [TomlConstructor]
            public NewTypeWithUnsuitableMarkedConstructor(string foo, string bar)
            {
            }
        }
    }
}
