using Freem.Domain.Relations.Collections.Base;
using Freem.Domain.Tags.Identifiers;

namespace Freem.Domain.Relations.Collections;

public sealed class RelatedTagsIdentifiersCollection : RelatedEntitiesIdentifiersCollection<TagIdentifier>
{
    public const int MinTagsCount = 0;
    public const int MaxTagsCount = 64;

    public static RelatedTagsIdentifiersCollection Empty { get; } = new();
    
    public RelatedTagsIdentifiersCollection()
        : base([], MinTagsCount, MaxTagsCount)
    {
    }

    public RelatedTagsIdentifiersCollection(IEnumerable<TagIdentifier> identifiers)
        : base(identifiers, MinTagsCount, MaxTagsCount)
    {
    }

    public RelatedTagsIdentifiersCollection(params TagIdentifier[] identifiers)
        : base(identifiers, MinTagsCount, MaxTagsCount)
    {
    }

    public RelatedTagsIdentifiersCollection(IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> collection)
        : this(collection.Identifiers)
    {
    }

    public void Update(RelatedTagsIdentifiersCollection other)
    {
        base.Update(other);
    }
}