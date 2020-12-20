using System.Collections.Immutable;
using Nageli;
using Nageli.Features.TaggedUnion;

internal static class Program
{
    private static void Main()
    {
        var options = TomlSerializerOptions.Default
            .WithPropertyNamingPolicy(TomlNamingPolicy.SnakeCase)
            .WithAbsentValuesPolicy(TomlAbsentValuesPolicy.Disallow)
            .AddTaggedUnionConverter(TomlTaggedUnionOptions.Default.WithTagNamingPolicy(TomlNamingPolicy.SnakeCase));

        const string toml = "[[email_delivery]]\n" +
                            "type = \"null\"\n" +
                            "[[email_delivery]]\n" +
                            "type = \"pickup\"\ndirectory_path = \"/foo/bar\"";

        var configuration = TomlSerializer.Deserialize<Configuration>(toml, options);
    }

    internal sealed record Configuration(IImmutableSet<EmailDelivery> EmailDelivery);

    [TomlTaggedUnion]
    internal abstract record EmailDelivery
    {
        public sealed record Null : EmailDelivery;

        public sealed record Pickup(string DirectoryPath) : EmailDelivery;
    }
}
