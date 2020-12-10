using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class ObjectConverter<T> : TomlConverter<T>
        where T : notnull
    {
        private readonly ConstructorInfo _constructor;
        private readonly IReadOnlyList<(ParameterInfo Info, TomlConverter Converter)> _parameterConverters;

        public ObjectConverter(ConstructorInfo constructor, IReadOnlyList<(ParameterInfo Info, TomlConverter Converter)> parameterConverters)
        {
            _constructor = constructor;
            _parameterConverters = parameterConverters;
        }

        public override T ConvertFrom(TomlObject value, TomlSerializerOptions options)
        {
            if (value is TomlTable tomlTable)
            {
                return ConvertFrom(tomlTable, options);
            }

            throw new TomlException("Objects can only be converted from tables");
        }

        public override TomlObject ConvertTo(T value, TomlSerializerOptions options) => throw new NotImplementedException();

        private T ConvertFrom(TomlTable table, TomlSerializerOptions options)
        {
            var parameters = _parameterConverters.Select(p => ConvertParameter(table, p.Info, p.Converter, options)).ToArray();
            return (T)_constructor.Invoke(parameters);
        }

        private static object ConvertParameter(TomlTable table, ParameterInfo parameterInfo, TomlConverter converter, TomlSerializerOptions options)
        {
            var parameterName = parameterInfo.Name ?? throw new TomlException("Constructor parameter without name");
            var parameterType = parameterInfo.ParameterType;
            var propertyName = options.PropertyNamingPolicy.ConvertName(parameterName);

            // TODO: support nullable reference types

            return table.TryGetToml(propertyName, out var tomlObject)
                ? converter.ConvertFrom(tomlObject, parameterType, options)
                : converter.ConvertFromAbsent(parameterType, options);
        }
    }
}
