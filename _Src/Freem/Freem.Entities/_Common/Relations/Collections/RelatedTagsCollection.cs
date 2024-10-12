using Freem.Entities.Common.Relations.Collections.Base;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;

// ReSharper disable once CheckNamespace
namespace Freem.Entities.Common.Relations.Collections;

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

    public void Update(RelatedTagsCollection other)
    {
        base.Update(other);
    }
}
