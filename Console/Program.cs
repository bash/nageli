using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Funcky.Monads;
using Nageli;
using Nageli.Features.Option;
using Nageli.Features.TaggedUnion;

namespace NaegeliConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = TomlSerializerOptions.Default
                .WithPropertyNamingPolicy(TomlNamingPolicy.SnakeCase)
                .WithMissingValuesPolicy(MissingValuesPolicy.Disallow)
                .AddTaggedUnionConverter()
                .AddOptionConverter();
            var person1 = TomlSerializer.Deserialize<Person>("first_name = \"Jane\"\nlast_name = \"Doe\"\nmessage=\"Hello World\"", options);
            Console.WriteLine(person1);
            var person2 = TomlSerializer.Deserialize<Person>("first_name = \"Jane\"\nlast_name = \"Doe\"\nage=25", options);
            Console.WriteLine(person2);

            var dict1 = TomlSerializer.Deserialize<IDictionary<string, string>>(
                "first_name = \"Jane\"\nlast_name = \"Doe\"\nmessage=\"Hello World\"");

            var dict2 = TomlSerializer.Deserialize<ConcurrentDictionary<string, string>>(
                "first_name = \"Jane\"\nlast_name = \"Doe\"\nmessage=\"Hello World\"");

            var dict3 = TomlSerializer.Deserialize<SortedDictionary<string, string>>(
                "first_name = \"Jane\"\nlast_name = \"Doe\"\nmessage=\"Hello World\"");

            var taggedUnion1 = TomlSerializer.Deserialize<EmailDelivery>(
                "type = \"null\"", options);

            var taggedUnion2 = TomlSerializer.Deserialize<EmailDelivery>(
                "type = \"pickup\"\ndirectory_path = \"/foo/bar\"", options);

            var taggedUnionVariant = TomlSerializer.Deserialize<EmailDelivery.Pickup>(
                "directory_path = \"/foo/bar\"", options);

            Console.WriteLine(taggedUnion1);
            Console.WriteLine(taggedUnion2);
            Console.WriteLine(taggedUnionVariant);
        }
    }

    internal record Person(string FirstName, string LastName, int? Age, Option<string> Message);

    [TomlTaggedUnion(Tag = "type")]
    internal abstract record EmailDelivery
    {
        [TomlTag("null")]
        public sealed record Null : EmailDelivery
        {
        }

        [TomlTag("pickup")]
        public sealed record Pickup(string DirectoryPath) : EmailDelivery
        {
        }
    }
}
