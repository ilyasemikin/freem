namespace Freem.Entities.Users.Comparers;

public sealed class UserPasswordCredentialsEqualityComparer : IEqualityComparer<UserPasswordCredentials>
{
    public static IEqualityComparer<UserPasswordCredentials> Instance { get; } = new UserPasswordCredentialsEqualityComparer();
    
    public bool Equals(UserPasswordCredentials? x, UserPasswordCredentials? y)
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
            x.Login.Equals(y.Login) &&
            x.PasswordHash.Equals(y.PasswordHash);
    }

    public int GetHashCode(UserPasswordCredentials obj)
    {
        return HashCode.Combine(obj.Login, obj.PasswordHash);
    }
}