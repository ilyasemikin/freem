using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Records.Comparers;

public sealed class RecordEqualityComparer : IEqualityComparer<Record>
{
    public bool Equals(Record? x, Record? y)
    {
        if (ReferenceEquals(x, y)) 
            return true;
        if (x is null) 
            return false;
        if (y is null) 
            return false;
        if (x.GetType() != y.GetType()) 
            return false;
        return 
            x.Id.Equals(y.Id) && 
            x.UserId.Equals(y.UserId) && 
            x.Period == y.Period &&
            x.Name == y.Name &&
            x.Description == y.Description &&
            IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals(x.Activities, y.Activities) &&
            IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals(x.Tags, y.Tags);
    }

    public int GetHashCode(Record obj)
    {
        return HashCode.Combine(obj.Id, obj.UserId, obj.Activities, obj.Tags, obj.Period);
    }
}