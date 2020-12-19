using System;
using Funcky.Monads;
using Nageli;
using Nageli.Features.Option;
using Nageli.Features.TaggedUnion;

internal static class Program
{
    private static void Main()
    {
        var options = TomlSerializerOptions.Default
            .WithPropertyNamingPolicy(TomlNamingPolicy.SnakeCase)
            .WithAbsentValuesPolicy(AbsentValuesPolicy.Disallow)
            .AddTaggedUnionConverter(TomlTaggedUnionOptions.Default.WithTagNamingPolicy(TomlNamingPolicy.SnakeCase))
            .AddOptionConverter();

        var taggedUnion1 = TomlSerializer.Deserialize<EmailDelivery>(
            "type = \"null\"", options);

        Console.WriteLine(taggedUnion1);

        var taggedUnion2 = TomlSerializer.Deserialize<EmailDelivery>(
            "type = \"pickup\"\ndirectory_path = \"/foo/bar\"", options);

        Console.WriteLine(taggedUnion2);
    }

    [TomlTaggedUnion]
    internal abstract record EmailDelivery
    {
        public sealed record Null : EmailDelivery;

        public sealed record Pickup(string DirectoryPath) : EmailDelivery;
    }
}
