using Freem.Domain.Entities.Tags;

namespace Freem.Domain.UseCases.Contracts.Tags.Get;

public sealed class GetTagResponse
{
    public Tag Tag { get; }

    public GetTagResponse(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        Tag = tag;
    }
}