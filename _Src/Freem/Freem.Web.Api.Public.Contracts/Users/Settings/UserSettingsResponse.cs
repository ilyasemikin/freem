using Freem.Entities.Users.Models;

namespace Freem.Web.Api.Public.Contracts.Users.Settings;

public sealed class UserSettingsResponse
{
    public DayUtcOffset DayUtcOffset { get; }
    
    public UserSettingsResponse(DayUtcOffset dayUtcOffset)
    {
        ArgumentNullException.ThrowIfNull(dayUtcOffset);
        
        DayUtcOffset = dayUtcOffset;
    }
}