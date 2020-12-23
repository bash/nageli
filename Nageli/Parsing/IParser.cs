using Nageli.Model;

namespace Nageli.Parsing
{
    internal interface IParser
    {
        TomlObject Parse(string toml);
    }
}
