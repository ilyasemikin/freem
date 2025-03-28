﻿using Freem.Entities.Users.Models;

namespace Freem.Web.Api.Public.Contracts.Users.Settings;

public sealed class UpdateUserSettingsRequest
{
    public UpdateField<DayUtcOffset>? DayUtcOffset { get; init; }

    public bool HasChanges()
    {
        return DayUtcOffset is not null;
    }
}