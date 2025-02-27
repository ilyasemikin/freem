using Freem.Entities.Users.Models;

namespace Freem.Entities.Users;

public sealed class UserSettings
{
    public static UserSettings Default { get; } = new();
    
    public DayUtcOffset DayUtcOffset { get; set; } = DayUtcOffset.Default;
}