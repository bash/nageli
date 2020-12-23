using System;
using Tomlyn;
using Tomlyn.Model;

namespace Nageli
{
    public static class TomlSerializer
    {
        public static TValue Deserialize<TValue>(string toml, ITomlToSerializerContext? context = null)
            => (TValue)Deserialize(Parse(toml), typeof(TValue), context);

        public static TValue Deserialize<TValue>(TomlObject toml, ITomlToSerializerContext? context = null)
            => (TValue)Deserialize(toml, typeof(TValue), context);

        public static object Deserialize(string toml, Type type, ITomlToSerializerContext? context = null)
            => Deserialize(Parse(toml), type, context);

        public static object Deserialize(TomlObject toml, Type type, ITomlToSerializerContext? context = null)
        {
            var contextOrDefault = context?.ToSerializerContext() ?? TomlSerializerContext.Create();
            var converter = contextOrDefault.GetConverter(type);
            return converter.ConvertFrom(toml, contextOrDefault);
        }

        private static TomlObject Parse(string toml) => Toml.Parse(toml).ToModel();
    }
}
