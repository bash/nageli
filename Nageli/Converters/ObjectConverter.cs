using System;
using System.Linq;
using System.Reflection;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class ObjectConverter : TomlConverter
    {
        public override bool CanConvert(Type type) => true;

        public override object ConvertFrom(TomlObject value, Type typeToConvert, TomlSerializerOptions options)
        {
            if (value is TomlTable tomlTable)
            {
                return ConvertFrom(tomlTable, typeToConvert, options);
            }

            throw new TomlException("Objects can only be converted from tables");
        }

        public override TomlObject ConvertTo(object value, TomlSerializerOptions options) => throw new NotImplementedException();

        private static object ConvertFrom(TomlTable table, Type typeToConvert, TomlSerializerOptions options)
        {
            var constructor = typeToConvert.GetConstructors().FirstOrDefault()
                ?? throw new TomlException($"No public constructor found for {typeToConvert}");
            var parameters = constructor.GetParameters().Select(p => ConvertParameter(table, p, options)).ToArray();
            return constructor.Invoke(parameters);
        }

        private static object? ConvertParameter(TomlTable table, ParameterInfo parameterInfo, TomlSerializerOptions options)
        {
            var parameterName = parameterInfo.Name ?? throw new TomlException("Constructor parameter without name");
            var parameterType = parameterInfo.ParameterType;
            var propertyName = options.PropertyNamingPolicy.ConvertName(parameterName);
            var parameterConverter = options.GetConverter(parameterType);
            
            // TODO: support nullable reference types

            return table.TryGetToml(propertyName, out var tomlObject)
                ? parameterConverter.ConvertFrom(tomlObject, parameterType, options)
                : parameterConverter.ConvertFromAbsent(parameterType, options);
        }
    }
}
