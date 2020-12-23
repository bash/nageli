using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Nageli.Model
{
    public sealed record TomlArray : TomlObject, IReadOnlyList<TomlObject>
    {
        public static TomlArray Empty { get; } = new(ImmutableArray<TomlObject>.Empty);

        private readonly IImmutableList<TomlObject> _list;

        private TomlArray(IImmutableList<TomlObject> list) => _list = list;

        [Pure]
        public TomlArray Add(TomlObject value) => new(_list.Add(value));

        [Pure]
        public TomlArray AddRange(IEnumerable<TomlObject> values) => new(_list.AddRange(values));

        public IEnumerator<TomlObject> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _list.Count;

        public TomlObject this[int index] => _list[index];

        public bool Equals(TomlArray? other) => other is not null && this.SequenceEqual(other);
    }
}
