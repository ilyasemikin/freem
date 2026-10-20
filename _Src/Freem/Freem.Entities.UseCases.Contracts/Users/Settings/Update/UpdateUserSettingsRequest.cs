using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Users.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Users.Settings.Update;

public sealed class UpdateUserSettingsRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Settings.Update;

    public UpdateField<DayUtcOffset>? UtcOffset { get; init; }

    [MemberNotNullWhen(true, nameof(UtcOffset))]
    public bool HasChanges()
    {
        return UtcOffset is not null;
    }
}