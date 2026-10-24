using Freem.Domain.Entities.Tags;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Tags.Search;

public sealed class SearchTagByNameResponse
{
    public IReadOnlyList<Tag> Tags { get; }
    public TotalCount TotalCount { get; }

    public SearchTagByNameResponse(IReadOnlyList<Tag> tags, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(totalCount);

        Tags = tags;
        TotalCount = totalCount;
    }
}