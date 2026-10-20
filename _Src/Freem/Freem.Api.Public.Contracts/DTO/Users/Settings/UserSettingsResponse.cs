using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.DTO.Users.Settings;

/// <summary>Settings of the current user.</summary>
public sealed class UserSettingsResponse
{
    /// <summary>Shift of the start of the user's day relative to 00:00 UTC (zero by default); defines day boundaries in statistics.</summary>
    public DayUtcOffset DayUtcOffset { get; }
    
    public UserSettingsResponse(DayUtcOffset dayUtcOffset)
    {
        ArgumentNullException.ThrowIfNull(dayUtcOffset);
        
        DayUtcOffset = dayUtcOffset;
    }
}