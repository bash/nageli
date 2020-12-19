using System;
using Tomlyn.Model;

namespace Nageli.Features.NewType
{
    internal sealed class NewTypeConverter<TNewType> : ITomlConverter<TNewType>
        where TNewType : notnull
    {
        private readonly NewTypeMetadata _metadata;

        public NewTypeConverter(NewTypeMetadata metadata) => _metadata = metadata;

        public TNewType ConvertFrom(TomlObject value, TomlSerializerOptions options)
            => (TNewType)_metadata.Constructor.Invoke(new[] { _metadata.InnerConverter.ConvertFrom(value, options) });

        public TomlObject ConvertTo(TNewType value, TomlSerializerOptions options) => throw new NotImplementedException();
    }
}
