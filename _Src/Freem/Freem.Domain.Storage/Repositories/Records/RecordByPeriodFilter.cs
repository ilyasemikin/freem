using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Users;
using Freem.Time;

namespace Freem.Domain.Storage.Repositories.Records;

public sealed class RecordByPeriodFilter : ILimitFilter
{
    private readonly Limit _limit;

    public Limit Limit
    {
        get => _limit;
        init => _limit = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public UserId UserId { get; }
    public DateTimePeriod Period { get; }
    public RecordId? AfterRecordId { get; }

    public RecordByPeriodFilter(UserId userId, DateTimePeriod period, RecordId? afterRecordId = null)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(period);

        _limit = Limit.Default;
        UserId = userId;
        Period = period;
        AfterRecordId = afterRecordId;
    }
}