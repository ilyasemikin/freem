using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections.Base;

namespace Freem.Entities.Relations.Collections;

public sealed class RelatedTagsCollection : RelatedEntitiesCollection<Tag, TagIdentifier>
{
    public const int MaxTagsCount = 64;

    public RelatedTagsCollection(IEnumerable<TagIdentifier> identifiers, IEnumerable<Tag> entities) 
        : base(identifiers, entities, maxCount: MaxTagsCount)
    {
    }

    public RelatedTagsCollection()
        : this([], [])
    {
    }

    public RelatedTagsCollection(IEnumerable<TagIdentifier> identifiers)
        : this(identifiers, [])
    {
    }

    public RelatedTagsCollection(IEnumerable<Tag> entities)
        : this([], entities)
    {
    }
}
