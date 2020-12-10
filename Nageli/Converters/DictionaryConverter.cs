using System;
using System.Collections.Generic;
using System.Linq;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class DictionaryConverter<TValue> : TomlConverter<Dictionary<string, TValue>>
        where TValue : notnull
    {
        private readonly TomlConverter<TValue> _valueConverter;

        public DictionaryConverter(TomlSerializerOptions options)
            => _valueConverter = options.GetConverter<TValue>();

        public override Dictionary<string, TValue> ConvertFrom(TomlObject value, TomlSerializerOptions options)
        {
            if (value is TomlTable tomlTable)
            {
                return tomlTable.Keys.ToDictionary(
                    key => key,
                    key => tomlTable.TryGetToml(key, out var dictionaryValue)
                        ? _valueConverter.ConvertFrom(dictionaryValue, options)
                        : throw new InvalidOperationException("This should never happen"));
            }

            throw new TomlException();
        }

        public override TomlObject ConvertTo(Dictionary<string, TValue> value, TomlSerializerOptions options) => throw new NotImplementedException();
    }
}
