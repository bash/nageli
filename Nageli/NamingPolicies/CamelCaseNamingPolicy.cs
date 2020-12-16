namespace Nageli.NamingPolicies
{
    internal sealed class CamelCaseNamingPolicy : ITomlNamingPolicy
    {
        public string ConvertName(string name)
        {
            var word = TomlNamingPolicy.Default.ConvertName(name);
            return word.Length > 0
                ? word[..1].ToLower() + word[1..]
                : word;
        }
    }
}
