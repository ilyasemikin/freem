using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Users.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Users.Settings.Update;

public sealed class UpdateUserSettingsRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Settings.Update;

    public UpdateField<TimeZoneOffset>? TimeZoneOffset { get; init; }

    [MemberNotNullWhen(true, nameof(TimeZoneOffset))]
    public bool HasChanges()
    {
        return TimeZoneOffset is not null;
    }
}