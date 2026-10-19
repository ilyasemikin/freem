using Freem.Domain.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Domain.Entities.RunningRecords.Comparers;

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
            x.StartAt == y.StartAt &&
            x.Name == y.Name &&
            x.Description == y.Description &&
            IReadOnlyRelatedIdsCollection<ActivityId>.Equals(x.Activities, y.Activities) &&
            IReadOnlyRelatedIdsCollection<TagId>.Equals(x.Tags, y.Tags);
    }

    public int GetHashCode(RunningRecord obj)
    {
        return HashCode.Combine(obj.UserId, obj.Activities, obj.Tags, obj.StartAt);
    }
}