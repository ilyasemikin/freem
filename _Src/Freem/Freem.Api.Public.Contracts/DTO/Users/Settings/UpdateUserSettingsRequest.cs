using Freem.Domain.Users.Models;
using Freem.Api.Public.Contracts.Models;

namespace Freem.Api.Public.Contracts.DTO.Users.Settings;

/// <summary>Request body for updating user settings. Fields that are omitted stay unchanged.</summary>
public sealed class UpdateUserSettingsRequest
{
    /// <summary>New shift of the start of the user's day relative to 00:00 UTC.</summary>
    public UpdateField<DayUtcOffset>? DayUtcOffset { get; init; }

    /// <summary>Whether at least one field is set.</summary>
    public bool HasChanges()
    {
        return DayUtcOffset is not null;
    }
}