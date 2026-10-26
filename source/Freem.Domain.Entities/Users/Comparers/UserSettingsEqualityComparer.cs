namespace Freem.Domain.Entities.Users.Comparers;

public sealed class UserSettingsEqualityComparer : IEqualityComparer<UserSettings>
{
    public static UserSettingsEqualityComparer Instance { get; } = new();
    
    public bool Equals(UserSettings? x, UserSettings? y)
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
            x.TimeZoneOffset.Equals(y.TimeZoneOffset) &&
            x.WeekStart.Equals(y.WeekStart);
    }

    public int GetHashCode(UserSettings obj)
    {
        return HashCode.Combine(obj.TimeZoneOffset, obj.WeekStart);
    }
}