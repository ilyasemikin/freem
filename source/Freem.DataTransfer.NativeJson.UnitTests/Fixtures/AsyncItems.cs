namespace Freem.DataTransfer.NativeJson.UnitTests.Fixtures;

internal static class AsyncItems
{
    public static async IAsyncEnumerable<T> From<T>(IEnumerable<T> items)
    {
        foreach (var item in items)
            yield return await Task.FromResult(item);
    }
}
