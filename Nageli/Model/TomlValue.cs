namespace Nageli.Model
{
    public abstract record TomlValue<TValue> : TomlObject
    {
        public TomlValue(TValue value) => Value = value;

        public TValue Value { get; }
    }
}
