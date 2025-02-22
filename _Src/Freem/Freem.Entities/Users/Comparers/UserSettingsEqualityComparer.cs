namespace Freem.Entities.Users.Comparers;

public sealed class UserSettingsEqualityComparer : IEqualityComparer<UserSettings>
{
    public static IEqualityComparer<UserSettings> Instance { get; } = new UserSettingsEqualityComparer();
    
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
        return x.DayUtcOffset.Equals(y.DayUtcOffset);
    }

    public int GetHashCode(UserSettings obj)
    {
        return obj.DayUtcOffset.GetHashCode();
    }
}