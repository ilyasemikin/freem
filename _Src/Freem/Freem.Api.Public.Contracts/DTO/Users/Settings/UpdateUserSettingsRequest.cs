using Freem.Domain.Users.Models;
using Freem.Api.Public.Contracts.Models;

namespace Freem.Api.Public.Contracts.DTO.Users.Settings;

/// <summary>Request body for updating user settings. Fields that are omitted stay unchanged.</summary>
public sealed class UpdateUserSettingsRequest
{
    /// <summary>New offset of the user's time zone from UTC, whole minutes in [-12:00, +14:00].</summary>
    public UpdateField<TimeZoneOffset>? TimeZoneOffset { get; init; }

    /// <summary>Whether at least one field is set.</summary>
    public bool HasChanges()
    {
        return TimeZoneOffset is not null;
    }
}