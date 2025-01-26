namespace Freem.Entities.Users.Comparers;

public sealed class UserEqualityComparer : IEqualityComparer<User>
{
    public bool Equals(User? x, User? y)
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
            x.Nickname == y.Nickname &&
            UserSettingsEqualityComparer.Instance.Equals(x.Settings, y.Settings) &&
            UserPasswordCredentialsEqualityComparer.Instance.Equals(x.PasswordCredentials, y.PasswordCredentials);
    }

    public int GetHashCode(User obj)
    {
        return HashCode.Combine(obj.Id, obj.Nickname);
    }
}