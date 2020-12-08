using System.Text.RegularExpressions;

namespace Nageli
{
    internal sealed class UpperCamelCaseNamingPolicy : ITomlNamingPolicy
    {
        // Source: https://github.com/Humanizr/Humanizer/blob/2e0920bd14d633a730d4bc3686529442ad64e9c0/src/Humanizer/InflectorExtensions.cs#L72
        public string ConvertName(string name) => Regex.Replace(name, "(?:^|_| +)(.)", match => match.Groups[1].Value.ToUpper());
    }
}
