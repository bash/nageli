// using System;
// using Tomlyn.Model;
//
// namespace Nageli.Converters
// {
//     internal sealed class TomlObjectConverter : TomlConverter
//     {
//         public override bool CanConvert(Type type) => typeof(TomlObject).IsAssignableFrom(type);
//
//         public override object ConvertFrom(TomlObject value, Type typeToConvert, TomlSerializerOptions options) => value;
//
//         public override TomlObject ConvertTo(object value, TomlSerializerOptions options) => (TomlObject)value;
//     }
// }
