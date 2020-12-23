using System;
using System.Collections.Generic;
using Xunit;

namespace Nageli.Test
{
    public sealed class DynamicObjectConverterTest
    {
        [Fact]
        public void ListOfObjectCanBeDeserialized()
        {
            const string toml = "Dynamic = [10, 20, 30, 40]";
            Assert.Equal(
                new object[] { 10L, 20L, 30L, 40L },
                (IEnumerable<object>)TomlSerializer.Deserialize<Document>(toml).Dynamic);
        }

        [Theory]
        [MemberData(nameof(PrimitiveTypesCanBeDeserializedAsObjectData))]
        public void PrimitiveTypesCanBeDeserializedAsObject(string toml, object expected)
        {
            var value = TomlSerializer.Deserialize<Document>(toml).Dynamic;
            Assert.Equal(expected, value);
        }

        public static TheoryData<string, object> PrimitiveTypesCanBeDeserializedAsObjectData()
            => new()
            {
                { "Dynamic = 10", 10L },
                { "Dynamic = 10.5", 10.5 },
                { "Dynamic = true", true },
                { "Dynamic = false", false },
                { "Dynamic = \"string\"", "string" },
                { "Dynamic = 2000-01-02T07:32:00", new DateTime(2000, 1, 2, 7, 32, 0) },
            };

        private sealed record Document(object Dynamic);
    }
}
