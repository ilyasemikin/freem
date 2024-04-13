using Freem.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters.Base;

public abstract class BaseFilter<TSortField>
    where TSortField : Enum
{
    private readonly long _offset;
    private readonly long _limit;

    public long Offset
    {
        get => _offset;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(Offset));

            _offset = value;
        }
    }

    public long Limit
    {
        get => _limit;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(Limit));

            _limit = value;
        }
    }

    public required SortOptions<TSortField> Sorting { get; init; }
}
