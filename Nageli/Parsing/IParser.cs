using Nageli.Model;

namespace Nageli.Parsing
{
    internal interface IParser
    {
        TomlTable Parse(string toml);
    }
}
