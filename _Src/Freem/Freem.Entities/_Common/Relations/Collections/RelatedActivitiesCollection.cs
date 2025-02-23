using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Collections;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Relations.Collections;

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

    public RelatedActivitiesCollection(params ActivityIdentifier[] identifiers)
        : this(identifiers, [])
    {
    }

    public RelatedActivitiesCollection(IEnumerable<Activity> entities)
        : this([], entities)
    {
    }

    public RelatedActivitiesCollection(IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> collection)
        : this(collection.Identifiers, [])
    {
    }

    public void Update(RelatedActivitiesCollection other)
    {
        base.Update(other);
    }
}
