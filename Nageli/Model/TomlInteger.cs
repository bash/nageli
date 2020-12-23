namespace Nageli.Model
{
    public sealed record TomlInteger : TomlValue<long>
    {
        public TomlInteger(long value)
            : base(value)
        {
        }
    }
}
