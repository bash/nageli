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
        private readonly IReadOnlyList<CachedParameterInfo> _parameterConverters;

        public ObjectConverter(ConstructorInfo constructor, IReadOnlyList<CachedParameterInfo> parameterConverters)
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
            var parameters = _parameterConverters.Select(p => ConvertParameter(table, p, options)).ToArray();
            return (T)_constructor.Invoke(parameters);
        }

        private static object ConvertParameter(TomlTable table, CachedParameterInfo parameter, TomlSerializerOptions options)
        {
            var parameterName = parameter.Info.Name ?? throw new TomlException("Constructor parameter without name");
            var parameterType = parameter.Info.ParameterType;
            var propertyName = options.PropertyNamingPolicy.ConvertName(parameterName);

            // TODO: support nullable reference types

            return table.TryGetToml(propertyName, out var tomlObject)
                ? parameter.Converter.ConvertFrom(tomlObject, parameterType, options)
                : parameter.Converter.ConvertFromAbsent(parameterType, options);
        }
    }
}
