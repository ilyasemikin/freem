using System.Diagnostics.CodeAnalysis;

namespace Freem.Entities.UseCases.Contracts.Users.Settings.Update;

public sealed class UpdateUserSettingsRequest
{
    public UpdateField<TimeSpan>? UtcOffset { get; init; }

    [MemberNotNullWhen(true, nameof(UtcOffset))]
    public bool HasChanges()
    {
        return UtcOffset is not null;
    }
}