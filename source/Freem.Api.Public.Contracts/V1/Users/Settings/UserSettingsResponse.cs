using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.V1.Users.Settings;

/// <summary>Settings of the current user.</summary>
public sealed class UserSettingsResponse
{
    /// <summary>Offset of the user's time zone from UTC in [-12:00, +14:00] (zero by default); day D starts at D 00:00 UTC minus the offset.</summary>
    public TimeZoneOffset TimeZoneOffset { get; }

    /// <summary>First day of the week: Monday (by default) or Sunday.</summary>
    public WeekStart WeekStart { get; }
    
    public UserSettingsResponse(TimeZoneOffset timeZoneOffset, WeekStart weekStart)
    {
        ArgumentNullException.ThrowIfNull(timeZoneOffset);
        ArgumentNullException.ThrowIfNull(weekStart);
        
        TimeZoneOffset = timeZoneOffset;
        WeekStart = weekStart;
    }
}