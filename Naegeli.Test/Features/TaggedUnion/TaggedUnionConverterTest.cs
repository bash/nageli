using Nageli;
using Nageli.Features.TaggedUnion;
using Xunit;

namespace Naegeli.Test.Features.TaggedUnion
{
    public sealed class TaggedUnionConverterTest
    {
        [Fact]
        public void DeserializingATaggedUnionFromTomlWhereTagIsNotStringThrows()
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            Assert.Throws<TomlException>(() => TomlSerializer.Deserialize<EmailDelivery>("Type = 3", options));
        }

        [Fact]
        public void DeserializingATaggedUnionFromTomlWithMissingTagKeyThrows()
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            Assert.Throws<TomlException>(() => TomlSerializer.Deserialize<EmailDelivery>(string.Empty, options));
        }

        [Fact]
        public void DeserializingATaggedUnionFromTomlWithInvalidTagThrows()
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            Assert.Throws<TomlException>(() => TomlSerializer.Deserialize<EmailDelivery>("Type = \"Custom\"", options));
        }

        [Theory]
        [MemberData(nameof(DeserializesTaggedUnionWithDefaultOptionsData))]
        public void DeserializesTaggedUnionWithDefaultOptions(EmailDelivery expected, string toml)
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            Assert.Equal(expected, TomlSerializer.Deserialize<EmailDelivery>(toml, options));
        }

        public static TheoryData<EmailDelivery, string> DeserializesTaggedUnionWithDefaultOptionsData()
            => new()
            {
                { new EmailDelivery.Null(), "Type = \"Null\"" },
                {
                    new EmailDelivery.Pickup("/tmp/email-pickup"),
                    "Type = \"Pickup\"\nDirectoryPath = \"/tmp/email-pickup\""
                },
                {
                    new EmailDelivery.SmtpServer("localhost", 25),
                    "Type = \"SmtpServer\"\nHost = \"localhost\"\nPort = 25"
                },
            };

        [Theory]
        [MemberData(nameof(DeserializesTaggedUnionWithCustomTagNamingPolicyData))]
        public void DeserializesTaggedUnionWithCustomTagNamingPolicy(EmailDelivery expected, string toml)
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter(
                TomlTaggedUnionOptions.Default.WithTagNamingPolicy(TomlNamingPolicy.SnakeCase));
            Assert.Equal(expected, TomlSerializer.Deserialize<EmailDelivery>(toml, options));
        }

        public static TheoryData<EmailDelivery, string> DeserializesTaggedUnionWithCustomTagNamingPolicyData()
            => new()
            {
                { new EmailDelivery.Null(), "Type = \"null\"" },
                {
                    new EmailDelivery.Pickup("/tmp/email-pickup"),
                    "Type = \"pickup\"\nDirectoryPath = \"/tmp/email-pickup\""
                },
                {
                    new EmailDelivery.SmtpServer("localhost", 25),
                    "Type = \"smtp_server\"\nHost = \"localhost\"\nPort = 25"
                },
            };

        [Theory]
        [MemberData(nameof(DeserializesTaggedUnionWithCustomPropertyNamingPolicyData))]
        public void DeserializesTaggedUnionWithCustomPropertyNamingPolicy(EmailDelivery expected, string toml)
        {
            var options = TomlSerializerOptions.Default
                .WithPropertyNamingPolicy(TomlNamingPolicy.SnakeCase)
                .AddTaggedUnionConverter();
            Assert.Equal(expected, TomlSerializer.Deserialize<EmailDelivery>(toml, options));
        }

        public static TheoryData<EmailDelivery, string> DeserializesTaggedUnionWithCustomPropertyNamingPolicyData()
            => new()
            {
                { new EmailDelivery.Null(), "type = \"Null\"" },
                {
                    new EmailDelivery.Pickup("/tmp/email-pickup"),
                    "type = \"Pickup\"\ndirectory_path = \"/tmp/email-pickup\""
                },
                {
                    new EmailDelivery.SmtpServer("localhost", 25),
                    "type = \"SmtpServer\"\nhost = \"localhost\"\nport = 25"
                },
            };

        [Fact]
        public void NonAbstractUnionTypeCanBeDeserialized()
        {
            var options = TomlSerializerOptions.Default
                .AddTaggedUnionConverter();
            Assert.Equal(
                new NonAbstractUnion.Variant(),
                TomlSerializer.Deserialize<NonAbstractUnion>("Type = \"Variant\"", options));
        }

        [Theory]
        [MemberData(nameof(DeserializesTaggedUnionWithMinimalAttributeConfigurationData))]
        public void DeserializesTaggedUnionWithMinimalAttributeConfiguration(UpdateMode expected, string toml)
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            Assert.Equal(expected, TomlSerializer.Deserialize<UpdateMode>(toml, options));
        }

        [Theory]
        [InlineData("Type = \"" + nameof(UnionWithInvalidNestedTypes.Abstract) + "\"")]
        [InlineData("Type = \"" + nameof(UnionWithInvalidNestedTypes.Detached) + "\"")]
        public void DeserializingInvalidNestedTypeDoesNotWork(string toml)
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            Assert.Throws<TomlException>(() => TomlSerializer.Deserialize<UnionWithInvalidNestedTypes>(toml, options));
        }

        [Fact]
        public void DeserializingVariantsDirectlyWorksWithoutDiscriminator()
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            const string toml = "ChannelName = \"beta\"";
            Assert.Equal(new UpdateMode.Latest("beta"), TomlSerializer.Deserialize<UpdateMode.Latest>(toml, options));
        }

        [Fact]
        public void DeserializingVariantsDirectlyIgnoresNonMatchingDiscriminator()
        {
            var options = TomlSerializerOptions.Default.AddTaggedUnionConverter();
            const string toml = "Type = \"Pinned\"\nChannelName = \"beta\"";
            Assert.Equal(new UpdateMode.Latest("beta"), TomlSerializer.Deserialize<UpdateMode.Latest>(toml, options));
        }

        public static TheoryData<UpdateMode, string> DeserializesTaggedUnionWithMinimalAttributeConfigurationData()
            => new()
            {
                { new UpdateMode.Auto(), "Type = \"Auto\"" },
                {
                    new UpdateMode.Pinned("1.4.2"),
                    "Type = \"Pinned\"\nVersion = \"1.4.2\""
                },
                {
                    new UpdateMode.Latest("beta"),
                    "Type = \"Latest\"\nChannelName = \"beta\""
                },
            };

        [TomlTaggedUnion(Tag = "Type")]
        public abstract record EmailDelivery
        {
            [TomlRename(nameof(Null))]
            public sealed record Null : EmailDelivery
            {
            }

            [TomlRename(nameof(Pickup))]
            public sealed record Pickup(string DirectoryPath) : EmailDelivery
            {
            }

            [TomlRename(nameof(SmtpServer))]
            public sealed record SmtpServer(string Host, int Port) : EmailDelivery
            {
            }
        }

        [TomlTaggedUnion(Tag = "Type")]
        public record NonAbstractUnion
        {
            public sealed record Variant : NonAbstractUnion;
        }

        [TomlTaggedUnion]
        public abstract record UpdateMode
        {
            public sealed record Auto : UpdateMode;

            public sealed record Pinned(string Version) : UpdateMode;

            public sealed record Latest(string ChannelName) : UpdateMode;
        }

        [TomlTaggedUnion]
        public abstract record UnionWithInvalidNestedTypes
        {
            public abstract record Abstract : UnionWithInvalidNestedTypes;

            public sealed record Detached;
        }
    }
}
