using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Funcky.Monads;
using Funcky.Nageli;
using Nageli;

namespace NaegeliConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = TomlSerializerOptions.Default
                .WithPropertyNamingPolicy(TomlNamingPolicy.SnakeCase)
                .WithConverter(new OptionConverterFactory());
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
        }
    }

    internal record Person(string FirstName, string LastName, int? Age, Option<string> Message);
}
