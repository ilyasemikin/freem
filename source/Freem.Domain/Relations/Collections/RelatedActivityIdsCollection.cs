using Freem.Domain.Activities;
using Freem.Domain.Relations.Collections.Base;

namespace Freem.Domain.Relations.Collections;

public sealed class RelatedActivityIdsCollection : RelatedIdsCollection<ActivityId>
{
    public const int MinActivitiesCount = 1;
    public const int MaxActivitiesCount = 16;

    public RelatedActivityIdsCollection(IEnumerable<ActivityId> ids)
        : base(ids, MinActivitiesCount, MaxActivitiesCount)
    {
    }

    public RelatedActivityIdsCollection(params ActivityId[] ids)
        : base(ids, MinActivitiesCount, MaxActivitiesCount)
    {
    }

    public RelatedActivityIdsCollection(
        IReadOnlyRelatedIdsCollection<ActivityId> collection)
        : this(collection.Ids)
    {
    }

    public void Update(RelatedActivityIdsCollection other)
    {
        base.Update(other);
    }
}