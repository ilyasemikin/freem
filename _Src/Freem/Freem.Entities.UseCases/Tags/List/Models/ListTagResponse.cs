using System.Collections;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Tags.List.Models;

public sealed class ListTagResponse : IEnumerable<Tag>
{
    public IReadOnlyList<Tag> Tags { get; }
    public TotalCount TotalCount { get; }

    public ListTagResponse(IReadOnlyList<Tag> tags, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(totalCount);
        
        Tags = tags;
        TotalCount = totalCount;
    }
    
    public ListTagResponse(IEnumerable<Tag> tags, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(totalCount);

        Tags = tags.ToArray();
        TotalCount = totalCount;
    }

    public IEnumerator<Tag> GetEnumerator()
    {
        return Tags.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}