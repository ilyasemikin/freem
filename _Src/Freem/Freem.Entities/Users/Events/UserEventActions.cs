namespace Freem.Entities.Users.Events;

public static class UserEventActions
{
    public const string Registered = "registered";
    public const string LoginCredentialsChanged = "password_credentials_changed";
    public const string SignedIn = "signed_in";
    
    public static IReadOnlyList<string> All { get; } = [Registered, LoginCredentialsChanged, SignedIn];
}