using Nageli.Model;
using Nageli.Parsing;
using Xunit;

namespace Nageli.Test.Parsing
{
    public sealed class TomlynParserTest
    {
        [Fact]
        public void NestedKeysAreParsedCorrectly()
        {
            const string toml = "foo.bar.baz = 10\n" +
                                "foo.qux = 20\n" +
                                "[foo.bar]\n" +
                                "qux = 30";
            var parser = new TomlynParser();
            var expected = TomlTable.Empty
                .Add("foo", TomlTable.Empty
                    .Add("bar", TomlTable.Empty
                        .Add("baz", new TomlInteger(10L))
                        .Add("qux", new TomlInteger(30L)))
                    .Add("qux", new TomlInteger(20L)));
            Assert.Equal(expected, parser.Parse(toml));
        }

        [Fact]
        public void TableArraysAreParsedCorrectly()
        {
            const string toml = "[[foo.bar]]\n" +
                                "baz = 10\n" +
                                "[[foo.bar]]\n" +
                                "baz = 20";
            var parser = new TomlynParser();
            var expected = TomlTable.Empty
                .Add("foo", TomlTable.Empty
                    .Add("bar", TomlArray.Empty
                        .Add(TomlTable.Empty
                            .Add("baz", new TomlInteger(10L)))
                        .Add(TomlTable.Empty
                            .Add("baz", new TomlInteger(20L)))));
            Assert.Equal(expected, parser.Parse(toml));
        }
    }
}
