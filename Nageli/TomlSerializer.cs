using System;
using Tomlyn;
using Tomlyn.Model;

namespace Nageli
{
    public static class TomlSerializer
    {
        public static TValue Deserialize<TValue>(string toml, TomlSerializerOptions? options = null)
            => (TValue)Deserialize(Parse(toml), typeof(TValue), TomlSerializerContext.Create(options));

        public static TValue Deserialize<TValue>(TomlObject toml, TomlSerializerOptions? options = null)
            => (TValue)Deserialize(toml, typeof(TValue), TomlSerializerContext.Create(options));

        public static TValue Deserialize<TValue>(string toml, ITomlSerializerContext? context = null)
            => (TValue)Deserialize(Parse(toml), typeof(TValue), context);

        public static TValue Deserialize<TValue>(TomlObject toml, ITomlSerializerContext? context = null)
            => (TValue)Deserialize(toml, typeof(TValue), context);

        public static object Deserialize(string toml, Type type, TomlSerializerOptions? options = null)
            => Deserialize(Parse(toml), type, TomlSerializerContext.Create(options));

        public static object Deserialize(TomlObject toml, Type type, TomlSerializerOptions? options = null)
            => Deserialize(toml, type, TomlSerializerContext.Create(options));

        public static object Deserialize(string toml, Type type, ITomlSerializerContext? context = null)
            => Deserialize(Parse(toml), type, context);

        public static object Deserialize(TomlObject toml, Type type, ITomlSerializerContext? context = null)
        {
            var contextOrDefault = context ?? TomlSerializerContext.Create();
            var converter = contextOrDefault.GetConverter(type);
            return converter.ConvertFrom(toml, contextOrDefault);
        }

        private static TomlObject Parse(string toml) => Toml.Parse(toml).ToModel();
    }
}
