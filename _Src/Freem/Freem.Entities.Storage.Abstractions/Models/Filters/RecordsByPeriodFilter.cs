using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Entities.Users.Identifiers;
using Freem.Time.Models;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class RecordsByPeriodFilter : ILimitFilter
{
    public Limit Limit { get; init; }
    
    public UserIdentifier UserId { get; }
    public DateTimePeriod Period { get; }

    public RecordsByPeriodFilter(UserIdentifier userId, DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(period);
        
        UserId = userId;
        Period = period;
    }
}