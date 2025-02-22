using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Time.Models;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class RecordsByPeriodFilter : ILimitFilter
{
    private readonly Limit _limit;

    public Limit Limit
    {
        get => _limit;
        init => _limit = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public UserIdentifier UserId { get; }
    public DateTimePeriod Period { get; }

    public RecordsByPeriodFilter(UserIdentifier userId, DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(period);

        _limit = Limit.Default;
        UserId = userId;
        Period = period;
    }
}