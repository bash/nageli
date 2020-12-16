using System.Text.RegularExpressions;

namespace Nageli.NamingPolicies
{
    internal sealed class SnakeCaseNamingPolicy : ITomlNamingPolicy
    {
        public string ConvertName(string name) => Regex.Replace(name, "(?!^)([A-Z]+)", match => '_' + match.Groups[1].Value).ToLowerInvariant();
    }
}
