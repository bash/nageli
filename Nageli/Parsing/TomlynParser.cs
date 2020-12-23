using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Nageli.Model;
using Tomlyn;
using Tomlyn.Syntax;

namespace Nageli.Parsing
{
    internal sealed class TomlynParser : IParser
    {
        public TomlObject Parse(string toml) => MapToTomlModel(Toml.Parse(toml));

        private static TomlObject MapToTomlModel(DocumentSyntax documentSyntax)
            => documentSyntax.KeyValues.Select(MapToInsertOperation)
                .Concat(documentSyntax.Tables.SelectMany(MapToInsertOperations))
                .Aggregate((TomlObject)TomlTable.Empty, ApplyInsertOperation);

        private static IEnumerable<InsertOperation> MapToInsertOperations(TableSyntaxBase tableSyntax)
            => tableSyntax switch
            {
                TableSyntax table => MapToInsertOperations(table),
                TableArraySyntax array => Enumerable.Repeat(MapToInsertOperation(array), 1),
                _ => throw new NotSupportedException(),
            };

        private static IEnumerable<InsertOperation> MapToInsertOperations(TableSyntax tableSyntax)
        {
            var keyParts = GetKeyParts(tableSyntax.Name).ToImmutableArray();
            return from item in tableSyntax.Items
                let operation = MapToInsertOperation(item)
                select operation.PrependKeyParts(keyParts);
        }


        private static InsertOperation MapToInsertOperation(TableArraySyntax tableArraySyntax)
        {
            var keyParts = GetKeyParts(tableArraySyntax.Name).ToImmutableArray();

            return new InsertOperation(keyParts, previousValue =>
            {
                var array = previousValue switch
                {
                    null => TomlArray.Empty,
                    TomlArray tomlArray => tomlArray,
                    _ =>  throw new TomlException(), // TODO: improve exception message
                };

                return array.Add(
                    tableArraySyntax.Items
                        .Select(MapToInsertOperation)
                        .Aggregate((TomlObject)TomlTable.Empty, ApplyInsertOperation));
            });
        }

        private static InsertOperation MapToInsertOperation(KeyValueSyntax keyValueSyntax)
            => new(
                KeyParts: GetKeyParts(keyValueSyntax.Key).ToImmutableList(),
                UpdateValue: previousValue =>
                {
                    // TODO: improve exception message
                    if (previousValue is not null) throw new TomlException("Duplicate key");
                    return MapToTomlModel(keyValueSyntax.Value);
                });

        private static IEnumerable<string> GetKeyParts(KeySyntax keySyntax)
            => Enumerable.Repeat(ToString(keySyntax.Key), 1)
                .Concat(keySyntax.DotKeys.Select(d => ToString(d.Key)));

        private static TomlObject ApplyInsertOperation(TomlObject? tomlObject, InsertOperation operation)
        {
            if (tomlObject is null) return ApplyInsertOperation(TomlTable.Empty, operation);

            return (tomlObject, operation.KeyParts.Count) switch
            {
                (TomlTable table, >1) => AddOrCreateTable(table, operation),
                (TomlTable table, 1) => UpdateTable(table, operation),
                _ => throw new TomlException(),
            };
        }

        private static TomlObject UpdateTable(TomlTable tomlTable, InsertOperation operation)
        {
            var key = operation.KeyParts[0];
            return tomlTable.Add(key, operation.UpdateValue(tomlTable.GetValueOrDefault(key)));
        }

        private static TomlObject AddOrCreateTable(TomlTable tomlTable, InsertOperation operation)
        {
            var (key, innerOperation) = operation.RemoveFirstKeyPart();
            var tableValue = tomlTable.GetValueOrDefault(key);
            var updatedTableValue = ApplyInsertOperation(tableValue, innerOperation);
            return tomlTable.Add(key, updatedTableValue);
        }

        private static string ToString(BareKeyOrStringValueSyntax keySyntax)
            => keySyntax switch
            {
                BareKeySyntax syntax => syntax.Key.Text,
                StringValueSyntax syntax => syntax.Value,
                _ => throw new NotSupportedException(),
            };

        private static TomlObject MapToTomlModel(ValueSyntax valueSyntax)
            => valueSyntax switch
            {
                StringValueSyntax stringValue => new TomlString(stringValue.Value),
                IntegerValueSyntax integerValue => new TomlInteger(integerValue.Value),
                BooleanValueSyntax booleanValue => new TomlBoolean(booleanValue.Value),
                FloatValueSyntax floatValue => new TomlFloat(floatValue.Value),
                // TODO: fix handling of local date times
                DateTimeValueSyntax dateTimeValue => new TomlDateTime(dateTimeValue.Value),
                InlineTableSyntax => throw new NotImplementedException(),
                ArraySyntax array => TomlArray.Empty.AddRange(array.Items.Select(i => i.Value).Select(MapToTomlModel)),
                _ => throw new NotSupportedException(),
            };

        private sealed record InsertOperation(
            IImmutableList<string> KeyParts,
            Func<TomlObject?, TomlObject> UpdateValue)
        {
            public InsertOperation PrependKeyParts(IEnumerable<string> keyPath)
                => new(
                    KeyParts: ImmutableArray.CreateRange(keyPath.Concat(KeyParts)),
                    UpdateValue);

            public (string, InsertOperation) RemoveFirstKeyPart()
                => (KeyParts[0], new InsertOperation(KeyParts.RemoveAt(0), UpdateValue));
        }
    }
}
