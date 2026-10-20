using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.DTO.Users.Settings;

public sealed class UserSettingsResponse
{
    public DayUtcOffset DayUtcOffset { get; }
    
    public UserSettingsResponse(DayUtcOffset dayUtcOffset)
    {
        ArgumentNullException.ThrowIfNull(dayUtcOffset);
        
        DayUtcOffset = dayUtcOffset;
    }
}