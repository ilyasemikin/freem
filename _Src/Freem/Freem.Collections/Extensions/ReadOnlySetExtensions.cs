namespace Freem.Collections.Extensions;

public static class ReadOnlySetExtensions
{
    public static (IEnumerable<T> FirstWithoutSecond, IEnumerable<T> SecondWithoutFirst) ExceptMutual<T>(this IReadOnlySet<T> first, IReadOnlySet<T> second)
    {
        return (first.Except(second), first.Except(second));
    }
}
