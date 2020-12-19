using System;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal class GuidConverter : ITomlConverter<Guid>
    {
        public Guid ConvertFrom(TomlObject value, TomlSerializerOptions options)
            => Guid.Parse(options.GetConverter<string>().ConvertFrom(value, options));

        public TomlObject ConvertTo(Guid value, TomlSerializerOptions options)
            => new TomlString(value.ToString());
    }
}
