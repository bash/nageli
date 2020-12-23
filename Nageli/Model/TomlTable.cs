using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Nageli.Model
{
    public sealed record TomlTable : TomlObject, IReadOnlyDictionary<string, TomlObject>
    {
        public static TomlTable Empty { get; } = new(
            order: ImmutableArray<string>.Empty,
            map: ImmutableDictionary<string, TomlObject>.Empty);

        private readonly IImmutableList<string> _order;
        private readonly IImmutableDictionary<string, TomlObject> _map;

        private TomlTable(IImmutableList<string> order, IImmutableDictionary<string, TomlObject> map)
        {
            _order = order;
            _map = map;
        }

        [Pure]
        public TomlTable Add(string key, TomlObject value)
            => new(
                order: _map.ContainsKey(key) ? _order : _order.Add(key),
                map: _map.SetItem(key, value));

        public IEnumerator<KeyValuePair<string, TomlObject>> GetEnumerator()
            => _order.Select(key => new KeyValuePair<string, TomlObject>(key, _map[key])).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _order.Count;

        public bool ContainsKey(string key) => _map.ContainsKey(key);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlObject value) => _map.TryGetValue(key, out value);

        public TomlObject this[string key] => _map[key];

        public IEnumerable<string> Keys => _order;

        public IEnumerable<TomlObject> Values => _order.Select(key => _map[key]);

        public bool Equals(TomlTable? other)
            => other is not null && this.SequenceEqual(other);
    }
}
