using Freem.Domain.Entities.Tags;
using Freem.Entities.UseCases.Contracts.Filter;

namespace Freem.Entities.UseCases.Contracts.Tags.List;

public sealed class ListTagResponse
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
}