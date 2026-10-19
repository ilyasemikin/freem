using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Relations.Collections.Base;

namespace Freem.Domain.Relations.Collections;

public sealed class RelatedActivitiesIdentifiersCollection : RelatedEntitiesIdentifiersCollection<ActivityIdentifier>
{
    public const int MinActivitiesCount = 1;
    public const int MaxActivitiesCount = 16;

    public RelatedActivitiesIdentifiersCollection(IEnumerable<ActivityIdentifier> identifiers)
        : base(identifiers, MinActivitiesCount, MaxActivitiesCount)
    {
    }

    public RelatedActivitiesIdentifiersCollection(params ActivityIdentifier[] identifiers)
        : base(identifiers, MinActivitiesCount, MaxActivitiesCount)
    {
    }

    public RelatedActivitiesIdentifiersCollection(
        IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> collection)
        : this(collection.Identifiers)
    {
    }

    public void Update(RelatedActivitiesIdentifiersCollection other)
    {
        base.Update(other);
    }
}