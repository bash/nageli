using System;
using System.Collections.Generic;
using System.Linq;

namespace Nageli.Converters
{
    internal sealed class DictionaryConverterFactory : ITomlConverterFactory
    {
        public bool CanConvert(Type type) => IsIDictionaryOfString(type) || type.GetInterfaces().Any(IsIDictionaryOfString);

        public ITomlConverter CreateConverter(Type typeToConvert, ITomlSerializerContext context)
        {
            var (instanceType, valueType) = GetTypes(typeToConvert);
            return (ITomlConverter) Activator.CreateInstance(
                typeof(DictionaryConverter<,>).MakeGenericType(valueType, typeToConvert),
                instanceType, context)!;
        }

        private (Type InstanceType, Type ValueType) GetTypes(Type typeToConvert)
        {
            if (IsIDictionaryOfString(typeToConvert))
            {
                var itemType = typeToConvert.GetGenericArguments()[1];
                return (typeof(Dictionary<,>).MakeGenericType(typeof(string), itemType), itemType);
            }

            var dictionaryInterface = typeToConvert.GetInterfaces().First(IsIDictionaryOfString);
            var valueType = dictionaryInterface.GetGenericArguments()[1];
            return (typeToConvert, valueType);
        }

        private static bool IsIDictionaryOfString(Type type)
            => type.IsGenericType
               && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)
               && type.GetGenericArguments()[0] == typeof(string);
    }
}
