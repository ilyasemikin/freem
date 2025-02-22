using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class EventsAfterTimeFilter : ILimitFilter
{
    public Limit Limit { get; init; }
    
    public UserIdentifier UserId { get; }
    public DateTimeOffset? After { get; }

    public EventsAfterTimeFilter(UserIdentifier userId, DateTimeOffset? after)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
        After = after;
    }
}