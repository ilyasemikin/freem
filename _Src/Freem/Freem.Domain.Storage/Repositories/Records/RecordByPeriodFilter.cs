using Freem.Domain.Records.Identifiers;
using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Users.Identifiers;
using Freem.Time.Models;

namespace Freem.Domain.Storage.Repositories.Records;

public sealed class RecordByPeriodFilter : ILimitFilter
{
    private readonly Limit _limit;

    public Limit Limit
    {
        get => _limit;
        init => _limit = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public UserIdentifier UserId { get; }
    public DateTimePeriod Period { get; }
    public RecordIdentifier? AfterRecordId { get; }

    public RecordByPeriodFilter(UserIdentifier userId, DateTimePeriod period, RecordIdentifier? afterRecordId = null)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(period);

        _limit = Limit.Default;
        UserId = userId;
        Period = period;
        AfterRecordId = afterRecordId;
    }
}