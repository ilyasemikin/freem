namespace Freem.Linq;

public static class AsyncEnumerableExtensions
{
    public static async Task<T[]> ToArrayAsync<T>(
        this IAsyncEnumerable<T> source, 
        CancellationToken cancellationToken = default)
    {
        var list = new List<T>();
        
        await foreach (var item in source.WithCancellation(cancellationToken))
            list.Add(item);
        
        return list.ToArray();
    }
}
