using System;
using Nageli.Features.NewType;
using Nageli.Model;
using Xunit;

namespace Nageli.Test.Features.NewType
{
    public sealed class NewTypeConverterTest
    {
        // TODO: Add tests with valid and invalid properties

        [Theory]
        [InlineData(typeof(NewTypeWithNoConstructors))]
        [InlineData(typeof(NewTypeWithNoSuitableConstructors))]
        [InlineData(typeof(NewTypeWithMoreThanOneSuitableConstructor))]
        [InlineData(typeof(NewTypeWithMoreThanOneMarkedConstructors))]
        [InlineData(typeof(NewTypeWithUnsuitableMarkedConstructor))]
        [InlineData(typeof(ValueTypeNewTypeWithNoConstructors))]
        public void CreatingTheConverterForNewTypeWithNoSuitableConstructorThrows(Type typeToConvert)
        {
            var context = TomlSerializerContext.Create(TomlSerializerOptions.Default.AddNewTypeConverter());
            Assert.Throws<TomlException>(() => context.GetConverter(typeToConvert));
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
                {
                    new TomlString("foo bar"),
                    new GenericNewType<string>("foo bar")
                },
                {
                    new TomlInteger(42),
                    new GenericNewType<long>(42)
                },
                {
                    new TomlInteger(42),
                    new GenericNewType<GenericNewType<GenericNewType<long>>>(new GenericNewType<GenericNewType<long>>(new GenericNewType<long>(42)))
                },
                {
                    TomlTable.Empty
                        .Add(nameof(Person.FirstName), new TomlString("Peter"))
                        .Add(nameof(Person.LastName), new TomlString("Pan")),
                    new GenericNewType<Person>(new Person("Peter", "Pan"))
                },
                {
                    new TomlString("foo bar"),
                    new ValueTypeNewType("foo bar")
                },
            };

        [TomlNewType]
        private sealed record NewTypeWithNoConstructors;

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

        [TomlNewType]
        private sealed record GenericNewType<T>
        {
            public GenericNewType(T value)
            {
                Value = value;
            }

            public T Value { get; }
        }

        private record Person(string FirstName, string LastName);

        [TomlNewType]
        private readonly struct ValueTypeNewType
        {
            public string Value { get; }

            public ValueTypeNewType(string value)
            {
                Value = value;
            }
        }

        [TomlNewType]
        private readonly struct ValueTypeNewTypeWithNoConstructors
        {
        }
    }
}
