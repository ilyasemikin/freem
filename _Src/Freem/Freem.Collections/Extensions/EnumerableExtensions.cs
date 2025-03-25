namespace Freem.Collections.Extensions;

public static class EnumerableExtensions
{
    public static (IEnumerable<T> FirstWithoutSecond, IEnumerable<T> SecondWithoutFirst) ExceptMutual<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        var firstSet = first.ToHashSet();
        var secondSet = second.ToHashSet();

        return firstSet.ExceptMutual(secondSet);
    }

    public static bool UnorderedEquals<T>(this IEnumerable<T> x, IEnumerable<T> y)
        where T : IEquatable<T>
    {
        return UnorderedEquals(x, y, EqualityComparer<T>.Default);
    }

    public static bool UnorderedEquals<T>(this IEnumerable<T> x, IEnumerable<T> y, IEqualityComparer<T> comparer)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(comparer);
        
        var counts = new Dictionary<T, int>(comparer);

        foreach (var value in x)
        {
            if (!counts.TryAdd(value, 1))
                counts[value]++;
        }

        foreach (var value in y)
        {
            if (!counts.ContainsKey(value))
                return false;

            counts[value]--;
        }
        
        return counts.All(p => p.Value == 0);
    }

    public static bool NullableUnorderedEquals<T>(this IEnumerable<T>? x, IEnumerable<T>? y)
        where T : IEquatable<T>
    {
        return
            x is null && y is null ||
            x is not null && y is not null &&
            x.UnorderedEquals(y);
    }

    public static bool NullableUnorderedEquals<T>(this IEnumerable<T>? x, IEnumerable<T>? y, IEqualityComparer<T> comparer)
        where T : notnull
    {
        return
            x is null && y is null ||
            x is not null && y is not null &&
            x.UnorderedEquals(y, comparer);
    }
}
