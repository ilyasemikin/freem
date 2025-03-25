namespace Freem.Entities.Tags.Comparers;

public sealed class TagEqualityComparer : IEqualityComparer<Tag>
{
    public bool Equals(Tag? x, Tag? y)
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
            x.Name.Equals(y.Name);
    }

    public int GetHashCode(Tag obj)
    {
        return HashCode.Combine(obj.Id, obj.UserId);
    }
}