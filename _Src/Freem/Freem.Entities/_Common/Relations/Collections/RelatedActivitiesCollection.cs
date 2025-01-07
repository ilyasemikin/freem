using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections.Base;

// ReSharper disable once CheckNamespace
namespace Freem.Entities.Common.Relations.Collections;

public sealed class RelatedActivitiesCollection : RelatedEntitiesCollection<Activity, ActivityIdentifier>
{
    public const int MinActivitiesCount = 1;
    public const int MaxActivitiesCount = 16;

    public RelatedActivitiesCollection(IEnumerable<ActivityIdentifier> identifiers, IEnumerable<Activity> entities) 
        : base(identifiers, entities, MinActivitiesCount, MaxActivitiesCount)
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

    public void Update(RelatedActivitiesCollection other)
    {
        base.Update(other);
    }
}
