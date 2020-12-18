using System;
using Tomlyn.Model;

namespace Nageli.Features.NewType
{
    internal sealed class TomlNewTypeConverter<TNewType> : TomlConverter<TNewType>
    {
        public override TNewType ConvertFrom(TomlObject value, TomlSerializerOptions options) => throw new NotImplementedException();

        public override TomlObject ConvertTo(TNewType value, TomlSerializerOptions options) => throw new NotImplementedException();
    }
}
