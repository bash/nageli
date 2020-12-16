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

        [TomlTaggedUnion(tag: "Type")]
        public abstract record EmailDelivery
        {
            [TomlTag(nameof(Null))]
            public sealed record Null : EmailDelivery
            {
            }

            [TomlTag(nameof(Pickup))]
            public sealed record Pickup(string DirectoryPath) : EmailDelivery
            {
            }

            [TomlTag(nameof(SmtpServer))]
            public sealed record SmtpServer(string Host, int Port) : EmailDelivery
            {
            }
        }
    }
}
