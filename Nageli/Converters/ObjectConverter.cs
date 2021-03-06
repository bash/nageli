using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nageli.Model;

namespace Nageli.Converters
{
    // TODO: objects with no properties should probably be convertible from absent values.
    internal sealed class ObjectConverter<T> : ITomlConverter<T>
        where T : notnull
    {
        private readonly ConstructorInfo _constructor;
        private readonly IReadOnlyList<CachedParameterInfo> _parameterConverters;

        public ObjectConverter(ConstructorInfo constructor, IReadOnlyList<CachedParameterInfo> parameterConverters)
        {
            _constructor = constructor;
            _parameterConverters = parameterConverters;
        }

        public T ConvertFrom(TomlObject value, ITomlSerializerContext context)
        {
            if (value is TomlTable tomlTable)
            {
                return ConvertFrom(tomlTable, context);
            }

            throw new TomlException("Objects can only be converted from tables");
        }

        public TomlObject ConvertTo(T value, ITomlSerializerContext context) => throw new NotImplementedException();

        private T ConvertFrom(TomlTable table, ITomlSerializerContext context)
        {
            var parameters = _parameterConverters.Select(p => ConvertParameter(table, p, context)).ToArray();
            return (T)_constructor.Invoke(parameters);
        }

        private static object? ConvertParameter(TomlTable table, CachedParameterInfo parameter, ITomlSerializerContext context)
        {
            var parameterName = parameter.Info.Name ?? throw new TomlException("Constructor parameter without name");
            var propertyName = context.ConvertPropertyName(parameterName);

            // TODO: support nullable reference types

            return table.TryGetValue(propertyName, out var tomlObject)
                ? parameter.Converter.ConvertFrom(tomlObject, context)
                : parameter.Converter.ConvertFromAbsent(context);
        }
    }
}
