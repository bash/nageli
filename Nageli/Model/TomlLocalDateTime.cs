using System;

namespace Nageli.Model
{
    public sealed record TomlLocalDateTime : TomlValue<DateTimeOffset>
    {
        public TomlLocalDateTime(DateTimeOffset value, DateTimeMask mask)
            : base(value)
        {
            Mask = mask;
        }

        public DateTimeMask Mask { get; }
    }
}
