using Nageli;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = TomlSerializerOptions.Default.WithPropertyNamingPolicy(TomlNamingPolicy.SnakeCase);
            var person1 = TomlSerializer.Deserialize<Person>("first_name = \"Jane\"\nlast_name = \"Doe\"", options);
            System.Console.WriteLine(person1);
            var person2 = TomlSerializer.Deserialize<Person>("first_name = \"Jane\"\nlast_name = \"Doe\"\nage=25", options);
            System.Console.WriteLine(person2);
        }
    }

    internal record Person(string FirstName, string LastName, int? Age);
}
