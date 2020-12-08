using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class Int32Converter : TomlConverter<int>
    {
        public override int ConvertFrom(TomlObject value, TomlSerializerOptions options)
        {
            if (value is TomlInteger tomlInteger)
            {
                // TODO: what happens if the value overflows?
                return (int)tomlInteger.Value;
            }

            throw new TomlException();
        }

        public override TomlObject ConvertTo(int value, TomlSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
