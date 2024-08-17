namespace Freem.Collections.Extensions;

public static class EnumerableExtensions
{
    public static (IEnumerable<T> FirstWithoutSecond, IEnumerable<T> SecondWithoutFirst) ExceptMutual<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        var firstSet = first.ToHashSet();
        var secondSet = second.ToHashSet();

        return firstSet.ExceptMutual(secondSet);
    }
}
