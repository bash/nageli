using System;
using Tomlyn;
using Tomlyn.Model;

namespace Nageli
{
    public static class TomlSerializer
    {
        public static TValue Deserialize<TValue>(string toml, TomlSerializerOptions? options = null)
            => Deserialize<TValue>(Toml.Parse(toml).ToModel(), options);

        public static TValue Deserialize<TValue>(TomlObject toml, TomlSerializerOptions? options = null)
            => (TValue)Deserialize(toml, typeof(TValue), options);

        public static object Deserialize(TomlObject toml, Type type, TomlSerializerOptions? options = null)
        {
            var context = TomlSerializerContext.Create(options ?? TomlSerializerOptions.Default);
            var converter = context.GetConverter(type);
            return converter.ConvertFrom(toml, context);
        }
    }
}
