using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Records;

namespace Freem.Domain.Entities.Records;

public sealed class RecordPeriodsCollection : IReadOnlyList<RecordPeriod>, IEquatable<RecordPeriodsCollection>
{
    private readonly List<RecordPeriod> _periods;

    public const int MinCount = 1;

    public int Count => _periods.Count;

    public RecordPeriod this[int index] => _periods[index];

    public RecordPeriodsCollection(IEnumerable<RecordPeriod> periods)
    {
        ArgumentNullException.ThrowIfNull(periods);

        var items = periods.ToList();
        if (!SortAndValidate(items))
            throw new ArgumentException(
                "Periods of record must be unique, not empty and must not overlap",
                nameof(periods));

        _periods = items;
    }

    public RecordPeriodsCollection(params RecordPeriod[] periods)
        : this((IEnumerable<RecordPeriod>)periods)
    {
    }

    public static bool TryCreate(IEnumerable<RecordPeriod> periods, [NotNullWhen(true)] out RecordPeriodsCollection? collection)
    {
        ArgumentNullException.ThrowIfNull(periods);

        var items = periods.ToList();
        if (!SortAndValidate(items))
        {
            collection = null;
            return false;
        }

        collection = new RecordPeriodsCollection(items);
        return true;
    }

    public bool Contains(RecordPeriodId id)
    {
        return _periods.Any(p => p.Id.Equals(id));
    }

    public bool TryAdd(RecordPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);

        var items = _periods.Append(period).ToList();
        return TryReplaceAll(items);
    }

    public bool TryReplace(RecordPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);

        var index = _periods.FindIndex(p => p.Id.Equals(period.Id));
        if (index < 0)
            return false;

        var items = _periods.ToList();
        items[index] = period;
        return TryReplaceAll(items);
    }

    public bool TryRemove(RecordPeriodId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var items = _periods.Where(p => !p.Id.Equals(id)).ToList();
        if (items.Count == _periods.Count)
            return false;

        return TryReplaceAll(items);
    }

    public void Update(RecordPeriodsCollection other)
    {
        ArgumentNullException.ThrowIfNull(other);

        _periods.Clear();
        _periods.AddRange(other._periods);
    }

    public IEnumerator<RecordPeriod> GetEnumerator()
    {
        return _periods.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals(RecordPeriodsCollection? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _periods.SequenceEqual(other._periods);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordPeriodsCollection other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var period in _periods)
            hash.Add(period);

        return hash.ToHashCode();
    }

    private bool TryReplaceAll(List<RecordPeriod> items)
    {
        if (!SortAndValidate(items))
            return false;

        _periods.Clear();
        _periods.AddRange(items);
        return true;
    }

    private static bool SortAndValidate(List<RecordPeriod> items)
    {
        if (items.Count < MinCount || items.Any(p => p is null))
            return false;

        if (items.Select(p => p.Id).Distinct().Count() != items.Count)
            return false;

        items.Sort((left, right) => left.Period.StartAt.CompareTo(right.Period.StartAt));

        for (var i = 1; i < items.Count; i++)
        {
            if (items[i - 1].Period.EndAt > items[i].Period.StartAt)
                return false;
        }

        return true;
    }
}
