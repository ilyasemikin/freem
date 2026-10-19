using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Storage.Repositories.Events;

public sealed class EventAfterTimeFilter : ILimitFilter
{
    public required Limit Limit { get; init; }
    
    public UserIdentifier UserId { get; }
    public DateTimeOffset? After { get; }

    public EventAfterTimeFilter(UserIdentifier userId, DateTimeOffset? after)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
        After = after;
    }
}