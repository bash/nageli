using System;
using System.Collections.Generic;

namespace Nageli.Converters
{
    internal sealed class DictionaryConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type)
            => type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Dictionary<,>) &&
               type.GetGenericArguments()[0] == typeof(string);

        public TomlConverter CreateConverter(Type typeToConvert, TomlSerializerOptions options)
            => (TomlConverter)Activator.CreateInstance(
                typeof(DictionaryConverter<>).MakeGenericType(),
                options)!;
    }
}
