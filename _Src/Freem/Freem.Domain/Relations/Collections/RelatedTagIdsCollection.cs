using Freem.Domain.Relations.Collections.Base;
using Freem.Domain.Tags;

namespace Freem.Domain.Relations.Collections;

public sealed class RelatedTagIdsCollection : RelatedIdsCollection<TagId>
{
    public const int MinTagsCount = 0;
    public const int MaxTagsCount = 64;

    public static RelatedTagIdsCollection Empty { get; } = new();
    
    public RelatedTagIdsCollection()
        : base([], MinTagsCount, MaxTagsCount)
    {
    }

    public RelatedTagIdsCollection(IEnumerable<TagId> ids)
        : base(ids, MinTagsCount, MaxTagsCount)
    {
    }

    public RelatedTagIdsCollection(params TagId[] ids)
        : base(ids, MinTagsCount, MaxTagsCount)
    {
    }

    public RelatedTagIdsCollection(IReadOnlyRelatedIdsCollection<TagId> collection)
        : this(collection.Ids)
    {
    }

    public void Update(RelatedTagIdsCollection other)
    {
        base.Update(other);
    }
}