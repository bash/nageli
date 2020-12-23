namespace Nageli.Model
{
    public sealed record TomlBoolean : TomlValue<bool>
    {
        public TomlBoolean(bool value)
            : base(value)
        {
        }
    }
}
