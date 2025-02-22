namespace Freem.Web.Api.Public.Contracts.Tags;

public class ListTagRequest
{
    public int Limit { get; }
    public int Offset { get; }

    public ListTagRequest(int limit, int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        
        Limit = limit;
        Offset = offset;
    }
}