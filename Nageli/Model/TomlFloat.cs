namespace Nageli.Model
{
    public sealed record TomlFloat : TomlValue<double>
    {
        public TomlFloat(double value)
            : base(value)
        {
        }
    }
}
