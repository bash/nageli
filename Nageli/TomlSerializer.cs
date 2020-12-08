using System;
using System.Linq;
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
            var optionsOrDefault = options ?? TomlSerializerOptions.Default;
            var converter = optionsOrDefault.GetConverter(type);
            return converter.ConvertFrom(toml, type, optionsOrDefault);
        }
    }
}
