using Freem.Domain.Users.Models;

namespace Freem.Domain.Entities.Users;

public sealed class UserSettings
{
    public static UserSettings Default { get; } = new();
    
    public TimeZoneOffset TimeZoneOffset { get; set; } = TimeZoneOffset.Default;
}