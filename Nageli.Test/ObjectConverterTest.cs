using System;
using Nageli.Model;
using Xunit;

namespace Nageli.Test
{
    public sealed class ObjectConverterTest
    {
        [Fact]
        public void EmptyObjectCanBeDeserializedFromEmptyTable()
        {
            var empty = TomlSerializer.Deserialize<Empty>(TomlTable.Empty);
            Assert.Equal(new Empty(), empty);
        }

        [Fact]
        public void TypeWithoutSettersCanBeDeserializedFromEmptyTable()
        {
            var value = TomlSerializer.Deserialize<Person>(TomlTable.Empty);
            Assert.Equal(new Person(null!, null!), value);
        }

        [Fact]
        public void TypeWithoutSettersCanBeDeserializedFromTable()
        {
            var value = TomlSerializer.Deserialize<Person>(
                TomlTable.Empty
                    .Add(nameof(Person.FirstName), new TomlString("Foo"))
                    .Add(nameof(Person.LastName), new TomlString("Bar")));
            Assert.Equal(new Person("Foo", "Bar"), value);
        }

        [Fact]
        public void NamingPolicyIsRespectedWhenDeserializingTypeWithoutSetters()
        {
            var options = TomlSerializerOptions.Default.WithPropertyNamingPolicy(TomlNamingPolicy.SnakeCase);
            var value = TomlSerializer.Deserialize<Person>(
                TomlTable.Empty
                    .Add("first_name", new TomlString("Foo"))
                    .Add("last_name", new TomlString("Bar")),
                options);
            Assert.Equal(new Person("Foo", "Bar"), value);
        }

        [Fact]
        public void DeserializingTypeWithoutSettersFromTableWithMissingKeysThrowsWhenAbsentValuesAreDisallowed()
        {
            var options = TomlSerializerOptions.Default.WithAbsentValuesPolicy(TomlAbsentValuesPolicy.Disallow);
            var input = TomlTable.Empty
                .Add(nameof(Person.FirstName), new TomlString("Foo"));
            Assert.Throws<TomlException>(() => TomlSerializer.Deserialize<Person>(input, options));
        }

        [Fact]
        public void ConstructorMarkedWithTomlConstructorIsUsedWhenDeserializingTypeWithoutSetters()
        {
            var input = TomlTable.Empty
                .Add(nameof(ClassWithMultipleConstructorsAndMarkedConstructor.FirstName), new TomlString("Foo"))
                .Add(nameof(ClassWithMultipleConstructorsAndMarkedConstructor.LastName), new TomlString("Bar"));
            Assert.Equal(
                new ClassWithMultipleConstructorsAndMarkedConstructor("Foo", "Bar"),
                TomlSerializer.Deserialize<ClassWithMultipleConstructorsAndMarkedConstructor>(input));
        }

        [Fact]
        public void ConstructorWithBiggestArityIsUsedWhenDeserializingTypeWithoutSettersAndMultipleConstructors()
        {
            var input = TomlTable.Empty
                .Add(nameof(ClassWithMultipleConstructors.FirstName), new TomlString("Foo"))
                .Add(nameof(ClassWithMultipleConstructors.LastName), new TomlString("Bar"));
            Assert.Equal(
                new ClassWithMultipleConstructors("Foo", "Bar"),
                TomlSerializer.Deserialize<ClassWithMultipleConstructors>(input));
        }

        [Fact]
        public void FirstConstructorIsUsedWhenDeserializingTypeWithoutSettersAndMultipleConstructorsOfSameArity()
        {
            var input = TomlTable.Empty
                .Add(nameof(ClassWithMultipleConstructorsOfSameArity.FirstName), new TomlString("Foo"))
                .Add(nameof(ClassWithMultipleConstructorsOfSameArity.LastName), new TomlString("Bar"));
            Assert.Equal(
                new ClassWithMultipleConstructorsOfSameArity("Foo", "Bar"),
                TomlSerializer.Deserialize<ClassWithMultipleConstructorsOfSameArity>(input));
        }

        [Fact]
        public void DeserializingATypeWithMultipleConstructorsMarkedAsTomlConstructorThrows()
        {
            var input = TomlTable.Empty
                .Add("A", new TomlString("Foo"))
                .Add("B", new TomlString("Bar"));
            Assert.Throws<TomlException>(() => TomlSerializer.Deserialize<ClassWithMultipleConstructorsMarkedAsTomlConstructor>(input));
        }

        [Fact]
        public void DeserializingATypeWithNoPublicConstructorsAndNoSettersThrows()
        {
            Assert.Throws<TomlException>(() => TomlSerializer.Deserialize<TypeWithNoPublicConstructor>(TomlTable.Empty));
        }

        [Fact]
        public void DefaultImplementationIsRespected()
        {
            var options = TomlSerializerOptions.Default.AddDefaultImplementation<IInterface, Person>();
            var value = TomlSerializer.Deserialize<IInterface>(
                TomlTable.Empty
                    .Add(nameof(Person.FirstName), new TomlString("Foo"))
                    .Add(nameof(Person.LastName), new TomlString("Bar")),
                options);
            Assert.Equal(new Person("Foo", "Bar"), value);
        }

        private interface IInterface
        {
        }

        private sealed record Empty;

        private sealed record Person(string FirstName, string LastName) : IInterface;

        private sealed record ClassWithMultipleConstructorsAndMarkedConstructor
        {
            public ClassWithMultipleConstructorsAndMarkedConstructor(string firstName, string middleName, string lastName) => throw new NotSupportedException();

            [TomlConstructor]
            public ClassWithMultipleConstructorsAndMarkedConstructor(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public ClassWithMultipleConstructorsAndMarkedConstructor(string firstName) => throw new NotSupportedException();

            public string FirstName { get; }

            public string MiddleName { get; } = string.Empty;

            public string LastName { get; }
        }

        private sealed record ClassWithMultipleConstructors
        {
            public string FirstName { get; }

            public string LastName { get; }

            public ClassWithMultipleConstructors(string firstName) => throw new NotSupportedException();

            public ClassWithMultipleConstructors(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }
        }

        private sealed record ClassWithMultipleConstructorsOfSameArity
        {
            public string FirstName { get; }

            public string LastName { get; }

            public ClassWithMultipleConstructorsOfSameArity(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public ClassWithMultipleConstructorsOfSameArity(string firstName, int age) => throw new NotSupportedException();
        }

        private sealed record ClassWithMultipleConstructorsMarkedAsTomlConstructor
        {
            [TomlConstructor]
            public ClassWithMultipleConstructorsMarkedAsTomlConstructor(string a, string b)
            {
            }

            [TomlConstructor]
            public ClassWithMultipleConstructorsMarkedAsTomlConstructor(string a)
            {
            }
        }

        private sealed class TypeWithNoPublicConstructor
        {
            private TypeWithNoPublicConstructor()
            {
            }
        }
    }
}
