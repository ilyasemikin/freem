namespace Freem.Collections.Extensions;

public static class EnumerableExtensions
{
    public static (IEnumerable<T> FirstWithoutSecond, IEnumerable<T> SecondWithoutFirst) ExceptMutual<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        var firstSet = first.ToHashSet();
        var secondSet = second.ToHashSet();

        return firstSet.ExceptMutual(secondSet);
    }

    public static bool UnorderedEquals<T>(this IEnumerable<T> first, IEnumerable<T> second)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        
        var counts = new Dictionary<T, int>();

        foreach (var value in first)
        {
            if (!counts.TryAdd(value, 1))
                counts[value]++;
        }

        foreach (var value in second)
        {
            if (!counts.ContainsKey(value))
                return false;

            counts[value]--;
        }

        return counts.All(x => x.Value == 0);
    }
}
