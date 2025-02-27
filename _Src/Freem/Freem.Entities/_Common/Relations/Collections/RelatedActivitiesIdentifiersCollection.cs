using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Relations.Collections.Base;

namespace Freem.Entities.Relations.Collections;

public sealed class RelatedActivitiesIdentifiersCollection : RelatedEntitiesIdentifiersCollection<ActivityIdentifier>
{
    public const int MinActivitiesCount = RelatedActivitiesCollection.MinActivitiesCount;
    public const int MaxActivitiesCount = RelatedActivitiesCollection.MaxActivitiesCount;

    public RelatedActivitiesIdentifiersCollection(IEnumerable<ActivityIdentifier> identifiers)
        : base(identifiers, MinActivitiesCount, MaxActivitiesCount)
    {
    }

    public RelatedActivitiesIdentifiersCollection(params ActivityIdentifier[] identifiers)
        : base(identifiers, MinActivitiesCount, MaxActivitiesCount)
    {
    }
}