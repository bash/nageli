using System;
using Nageli.Model;

namespace Nageli.Converters
{
    internal class GuidConverter : ITomlConverter<Guid>
    {
        public Guid ConvertFrom(TomlObject value, ITomlSerializerContext context)
            => Guid.Parse(context.GetConverter<string>().ConvertFrom(value, context));

        public TomlObject ConvertTo(Guid value, ITomlSerializerContext context)
            => new TomlString(value.ToString());
    }
}
