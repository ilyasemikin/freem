using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.DTO.Users.Settings;

/// <summary>Settings of the current user.</summary>
public sealed class UserSettingsResponse
{
    /// <summary>Offset of the user's time zone from UTC in [-12:00, +14:00] (zero by default); day D starts at D 00:00 UTC minus the offset.</summary>
    public TimeZoneOffset TimeZoneOffset { get; }
    
    public UserSettingsResponse(TimeZoneOffset timeZoneOffset)
    {
        ArgumentNullException.ThrowIfNull(timeZoneOffset);
        
        TimeZoneOffset = timeZoneOffset;
    }
}