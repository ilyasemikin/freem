using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Models.Users;

namespace Freem.Entities.UseCases.Contracts.Users.Settings.Update;

public sealed class UpdateUserSettingsRequest
{
    public UpdateField<DayUtcOffset>? UtcOffset { get; init; }

    [MemberNotNullWhen(true, nameof(UtcOffset))]
    public bool HasChanges()
    {
        return UtcOffset is not null;
    }
}