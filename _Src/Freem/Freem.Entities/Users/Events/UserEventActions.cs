namespace Freem.Entities.Users.Events;

public static class UserEventActions
{
    public const string SignedIn = "signed_in";
    
    public static IReadOnlyList<string> All { get; } = [SignedIn];
}