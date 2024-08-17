using System.Collections;

namespace Freem.Sorting;

public sealed class SortOptions<TSortField> : IReadOnlyList<SortOption<TSortField>>
    where TSortField : Enum
{
    private readonly SortOption<TSortField>[] _options;

    public SortOption<TSortField> this[int index] => _options[index];

    public int Count => _options.Length;

    public SortOptions(IEnumerable<SortOption<TSortField>> options)
    {
        _options = options.ToArray();

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(_options.Length, nameof(options));
    }

    public IEnumerator<SortOption<TSortField>> GetEnumerator()
    {
        foreach (var option in _options)
            yield return option;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
