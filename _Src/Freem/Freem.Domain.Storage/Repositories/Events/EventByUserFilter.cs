using Freem.Domain.Events;
using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Repositories.Events;

public sealed class EventByUserFilter : ILimitFilter, IOffsetFilter
{
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }

    public UserId UserId { get; }

    public EntityType? EntityType { get; init; }
    public string? EntityRef { get; init; }

    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }

    public EventByUserFilter(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}
