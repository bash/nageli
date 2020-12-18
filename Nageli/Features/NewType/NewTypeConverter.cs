using System;
using Tomlyn.Model;

namespace Nageli.Features.NewType
{
    internal sealed class NewTypeConverter<TNewType> : TomlConverter<TNewType>
        where TNewType : notnull
    {
        private readonly NewTypeMetadata _metadata;

        public NewTypeConverter(NewTypeMetadata metadata) => _metadata = metadata;

        public override TNewType ConvertFrom(TomlObject value, TomlSerializerOptions options)
            => (TNewType)_metadata.Constructor.Invoke(new[] { _metadata.InnerConverter.ConvertFrom(value, _metadata.InnerType, options) });

        public override TomlObject ConvertTo(TNewType value, TomlSerializerOptions options) => throw new NotImplementedException();
    }
}
