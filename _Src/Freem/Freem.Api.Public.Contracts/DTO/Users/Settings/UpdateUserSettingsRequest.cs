using Freem.Domain.Users.Models;
using Freem.Api.Public.Contracts.Models;

namespace Freem.Api.Public.Contracts.DTO.Users.Settings;

public sealed class UpdateUserSettingsRequest
{
    public UpdateField<DayUtcOffset>? DayUtcOffset { get; init; }

    public bool HasChanges()
    {
        return DayUtcOffset is not null;
    }
}