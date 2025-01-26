namespace Freem.Entities.Users;

public sealed class UserSettings
{
    public static UserSettings Default { get; } = new();
    
    public TimeSpan UtcOffset { get; set; }
}