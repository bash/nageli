using System;

namespace Nageli.Model
{
    public sealed record TomlDateTime : TomlValue<DateTime>
    {
        public TomlDateTime(DateTime value)
            : base(value)
        {
        }
    }
}
