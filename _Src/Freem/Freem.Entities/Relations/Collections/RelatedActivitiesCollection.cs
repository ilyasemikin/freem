using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections.Base;

namespace Freem.Entities.Relations.Collections;

public sealed class RelatedActivitiesCollection : RelatedEntitiesCollection<Activity, ActivityIdentifier>
{
    public const int MinActivitiesCount = 1;
    public const int MaxActivitiesCount = 16;

    public RelatedActivitiesCollection(IEnumerable<ActivityIdentifier> identifiers, IEnumerable<Activity> entities) 
        : base(identifiers, entities, MinActivitiesCount, MaxActivitiesCount)
    {
    }

    public RelatedActivitiesCollection()
        : this([], [])
    {
    }

    public RelatedActivitiesCollection(IEnumerable<ActivityIdentifier> identifiers)
        : this(identifiers, [])
    { 
    }

    public RelatedActivitiesCollection(IEnumerable<Activity> entities)
        : this(Enumerable.Empty<ActivityIdentifier>(), entities)
    {
    }
}
