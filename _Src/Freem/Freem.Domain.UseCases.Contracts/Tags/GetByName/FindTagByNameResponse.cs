using Freem.Domain.Entities.Tags;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Tags.GetByName;

public sealed class FindTagByNameResponse
{
    public IReadOnlyList<Tag> Tags { get; }
    public TotalCount TotalCount { get; }

    public FindTagByNameResponse(IReadOnlyList<Tag> tags, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(totalCount);

        Tags = tags;
        TotalCount = totalCount;
    }
}