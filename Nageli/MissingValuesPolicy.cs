namespace Nageli
{
    public enum MissingValuesPolicy
    {
        /// <summary>
        /// Uses the default value for the type (<c>null</c> for reference types).
        /// </summary>
        UseDefault,
        /// <summary>
        /// Throws when values are missing.
        /// </summary>
        Disallow,
    }
}
