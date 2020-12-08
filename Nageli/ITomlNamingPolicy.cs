
namespace Nageli
{
    public interface ITomlNamingPolicy
    {
        /// <summary>
        /// When overridden in a derived class, converts the specified name according to the policy.
        /// </summary>
        /// <param name="name">The name to convert.</param>
        /// <returns>The converted name.</returns>
        string ConvertName(string name);
    }
}
