using System;
using System.Collections.Generic;
using Tomlyn.Model;

namespace Nageli.Converters
{
    internal sealed class DynamicObjectConverter : ITomlConverter<object>
    {
        public TomlObject ConvertTo(object value, ITomlSerializerContext context)
        {
            // TODO: null handling
            var converter = context.GetConverter(value.GetType());
            return converter.ConvertTo(value, context);
        }

        public object ConvertFrom(TomlObject value, ITomlSerializerContext context)
        {
            var converter = value switch
            {
                TomlArray or TomlTableArray => (ITomlConverter)context.GetConverter<IEnumerable<object>>(),
                TomlString => context.GetConverter<string>(),
                TomlInteger => context.GetConverter<long>(),
                TomlBoolean => context.GetConverter<bool>(),
                TomlFloat => context.GetConverter<double>(),
                TomlDateTime => context.GetConverter<DateTime>(),
                TomlTable => throw new NotImplementedException(),
                _ => throw new NotSupportedException(),
            };
            return converter.ConvertFrom(value, context);
        }
    }
}
