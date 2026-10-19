using Freem.Domain.Users.Models;

namespace Freem.Domain.Entities.Users;

public sealed class UserSettings
{
    public static UserSettings Default { get; } = new();
    
    public DayUtcOffset DayUtcOffset { get; set; } = DayUtcOffset.Default;
}