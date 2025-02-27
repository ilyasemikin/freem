using Freem.Entities.Relations.Collections.Base;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Relations.Collections;

public sealed class RelatedTagsIdentifiersCollection : RelatedEntitiesIdentifiersCollection<TagIdentifier>
{
    public const int MinTagsCount = RelatedTagsCollection.MinTagsCount;
    public const int MaxTagsCount = RelatedTagsCollection.MaxTagsCount;
    
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
}