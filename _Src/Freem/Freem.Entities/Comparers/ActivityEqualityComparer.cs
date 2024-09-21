using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Comparers;

public sealed class ActivityEqualityComparer : IEqualityComparer<Activity>
{
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
            x.UserId.Equals(y.UserId) &&
            IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals(x.Tags, y.Tags);
    }

    public int GetHashCode(Activity obj)
    {
        return HashCode.Combine(obj.Id, obj.UserId, obj.Tags);
    }
}