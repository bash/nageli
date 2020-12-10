using System;
using System.Collections.Generic;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class DictionaryConverter<TValue, TDictionary> : TomlConverter<TDictionary>
        where TValue : notnull
        where TDictionary : IDictionary<string, TValue>
    {
        private readonly Type _instanceType;
        private readonly TomlConverter<TValue> _valueConverter;

        public DictionaryConverter(Type instanceType, TomlSerializerOptions options)
        {
            _valueConverter = options.GetConverter<TValue>();
            _instanceType = instanceType;
        }

        public override TDictionary ConvertFrom(TomlObject value, TomlSerializerOptions options)
            => value is TomlTable tomlTable
                ? ConvertFrom(tomlTable, options)
                : throw new TomlException();

        public override TomlObject ConvertTo(TDictionary value, TomlSerializerOptions options) => throw new NotImplementedException();

        private TDictionary ConvertFrom(TomlTable table, TomlSerializerOptions options)
        {
            var dictionary = (TDictionary)Activator.CreateInstance(_instanceType)!;

            foreach (var key in table.Keys)
            {
                var value = table.TryGetToml(key, out var dictionaryValue)
                    ? _valueConverter.ConvertFrom(dictionaryValue, options)
                    : throw new InvalidOperationException("This should never happen");
                dictionary[key] = value;
            }

            return dictionary;
        }
    }
}
