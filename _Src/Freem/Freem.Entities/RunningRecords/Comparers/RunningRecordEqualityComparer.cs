using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.RunningRecords.Comparers;

public sealed class RunningRecordEqualityComparer : IEqualityComparer<RunningRecord>
{
    public bool Equals(RunningRecord? x, RunningRecord? y)
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
            x.UserId.Equals(y.UserId) &&
            x.StartAt.Equals(y.StartAt) &&
            IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals(x.Activities, y.Activities) &&
            IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals(x.Tags, y.Tags);
    }

    public int GetHashCode(RunningRecord obj)
    {
        return HashCode.Combine(obj.UserId, obj.Activities, obj.Tags, obj.StartAt);
    }
}