using Freem.Entities.Users.Models;
using Freem.Web.Api.Public.Contracts.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Users.Settings;

public sealed class UpdateUserSettingsRequest
{
    public UpdateField<DayUtcOffset>? DayUtcOffset { get; init; }

    public bool HasChanges()
    {
        return DayUtcOffset is not null;
    }
}