using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Nageli.Model
{
    public sealed record TomlTable : TomlObject, IReadOnlyDictionary<string, TomlObject>
    {
        public static TomlTable Empty { get; } = new(
            order: ImmutableArray<KeyValuePair<string, TomlObject>>.Empty,
            map: ImmutableDictionary<string, TomlObject>.Empty);

        private readonly IImmutableList<KeyValuePair<string, TomlObject>> _order;
        private readonly IImmutableDictionary<string, TomlObject> _map;

        private TomlTable(IImmutableList<KeyValuePair<string, TomlObject>> order, IImmutableDictionary<string, TomlObject> map)
        {
            _order = order;
            _map = map;
        }

        [Pure]
        public TomlTable Add(string key, TomlObject value)
            => new(
                order: _order.Add(new KeyValuePair<string, TomlObject>(key, value)),
                map: _map.Add(key, value));

        public IEnumerator<KeyValuePair<string, TomlObject>> GetEnumerator() => _order.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _order.Count;

        public bool ContainsKey(string key) => _map.ContainsKey(key);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlObject value) => _map.TryGetValue(key, out value);

        public TomlObject this[string key] => _map[key];

        public IEnumerable<string> Keys => _map.Keys;

        public IEnumerable<TomlObject> Values => _map.Values;
    }
}
