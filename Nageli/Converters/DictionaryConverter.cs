using System;
using System.Collections.Generic;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class DictionaryConverter<TValue, TDictionary> : ITomlConverter<TDictionary>
        where TValue : notnull
        where TDictionary : IDictionary<string, TValue>
    {
        private readonly Type _instanceType;
        private readonly ITomlConverter<TValue> _valueConverter;

        public DictionaryConverter(Type instanceType, ITomlSerializerContext context)
        {
            _valueConverter = context.GetConverter<TValue>();
            _instanceType = instanceType;
        }

        public TDictionary ConvertFrom(TomlObject value, ITomlSerializerContext context)
            => value is TomlTable tomlTable
                ? ConvertFrom(tomlTable, context)
                : throw new TomlException();

        public TomlObject ConvertTo(TDictionary value, ITomlSerializerContext context) => throw new NotImplementedException();

        private TDictionary ConvertFrom(TomlTable table, ITomlSerializerContext context)
        {
            var dictionary = (TDictionary)Activator.CreateInstance(_instanceType)!;

            foreach (var key in table.Keys)
            {
                var value = table.TryGetToml(key, out var dictionaryValue)
                    ? _valueConverter.ConvertFrom(dictionaryValue, context)
                    : throw new InvalidOperationException("This should never happen");
                dictionary[key] = value;
            }

            return dictionary;
        }
    }
}
