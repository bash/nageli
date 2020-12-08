using System;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal class GuidConverter : TomlConverter<Guid>
    {
        public override Guid ConvertFrom(TomlObject value, TomlSerializerOptions options)
            => Guid.Parse(options.GetConverter<string>().ConvertFrom(value, options));

        public override TomlObject ConvertTo(Guid value, TomlSerializerOptions options)
            => new TomlString(value.ToString());
    }
}
