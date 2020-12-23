namespace Nageli.Model
{
    public sealed record TomlString : TomlValue<string>
    {
        public TomlString(string value)
            : base(value)
        {
        }
    }
}
