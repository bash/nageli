// using System;
// using System.Runtime.CompilerServices;
// using Tomlyn.Model;
//
// namespace Nageli.Converters
// {
//     internal sealed class EnumConverter<TEnum> : TomlConverter<TEnum>
//         where TEnum : struct, Enum
//     {
//         public override TEnum ConvertFrom(TomlObject value, TomlSerializerOptions options)
//             => value switch
//             {
//                 TomlString tomlString => Enum.Parse<TEnum>(tomlString.Value),
//                 TomlInteger { Value: var longValue } => Type.GetTypeCode(typeof(TEnum)) switch
//                 {
//                     TypeCode.Int32 => FromInteger((int) AssertWithinBounds(longValue, int.MinValue, int.MaxValue)),
//                     TypeCode.UInt32 => FromInteger((uint) AssertWithinBounds(longValue, uint.MinValue, uint.MaxValue)),
//                     TypeCode.UInt64 => FromInteger((ulong) AssertWithinBounds(longValue, int.MinValue, int.MaxValue)),
//                     TypeCode.Int32 => FromInteger((int) AssertWithinBounds(longValue, int.MinValue, int.MaxValue)),
//                     TypeCode.Int32 => FromInteger((int) AssertWithinBounds(longValue, int.MinValue, int.MaxValue)),
//                 },
//             };
//
//         public override TomlObject ConvertTo(TEnum value, TomlSerializerOptions options) => throw new NotImplementedException();
//
//         private long AssertWithinBounds(long value, long minValue, long maxValue)
//         {
//             if (value > maxValue || value < minValue)
//             {
//                 throw new TomlException();
//             }
//
//             return value;
//         }
//
//         private static TEnum FromInteger<TInteger>(TInteger value)
//             => Unsafe.As<TInteger, TEnum>(ref value);
//     }
// }
