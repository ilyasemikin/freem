using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Collections;
using Freem.Entities.Identifiers;
using Freem.Entities.Tags;

// ReSharper disable once CheckNamespace
namespace Freem.Entities.Relations.Collections;

public sealed class RelatedTagsCollection : RelatedEntitiesCollection<Tag, TagIdentifier>
{
    public const int MinTagsCount = 0;
    public const int MaxTagsCount = 64;

    public static RelatedTagsCollection Empty { get; } = new();
    
    public RelatedTagsCollection(IEnumerable<TagIdentifier> identifiers, IEnumerable<Tag> entities) 
        : base(identifiers, entities, MinTagsCount, MaxTagsCount)
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

    public RelatedTagsCollection(IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> collection)
        : this(collection.Identifiers)
    {
    }

    public void Update(RelatedTagsCollection other)
    {
        base.Update(other);
    }
}
