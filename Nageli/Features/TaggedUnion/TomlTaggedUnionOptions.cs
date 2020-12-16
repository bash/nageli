namespace Nageli.Features.TaggedUnion
{
    public sealed record TomlTaggedUnionOptions
    {
        public static TomlTaggedUnionOptions Default { get; } = new(
            tagNamingPolicy: TomlNamingPolicy.Default);

        public ITomlNamingPolicy TagNamingPolicy { get; }

        private TomlTaggedUnionOptions(ITomlNamingPolicy tagNamingPolicy)
        {
            TagNamingPolicy = tagNamingPolicy;
        }

        public TomlTaggedUnionOptions WithTagNamingPolicy(ITomlNamingPolicy namingPolicy)
            => ShallowClone(tagNamingPolicy: namingPolicy);

        private TomlTaggedUnionOptions ShallowClone(
            ITomlNamingPolicy? tagNamingPolicy = null)
            => new TomlTaggedUnionOptions(
                tagNamingPolicy: tagNamingPolicy ?? TagNamingPolicy);
    }
}
