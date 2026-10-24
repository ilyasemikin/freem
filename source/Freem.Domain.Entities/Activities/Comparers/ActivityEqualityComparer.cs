using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Domain.Entities.Activities.Comparers;

public sealed class ActivityEqualityComparer : IEqualityComparer<Activity>
{
    public static ActivityEqualityComparer Instance { get; } = new();

    public bool Equals(Activity? x, Activity? y)
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
            x.Name.Equals(y.Name) &&
            x.Color.Equals(y.Color) &&
            x.Status.Equals(y.Status) &&
            x.UserId.Equals(y.UserId) &&
            IReadOnlyRelatedIdsCollection<TagId>.Equals(x.Tags, y.Tags);
    }

    public int GetHashCode(Activity obj)
    {
        return HashCode.Combine(obj.Id, obj.UserId, obj.Tags);
    }
}