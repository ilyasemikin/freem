using Freem.Domain.Entities.Tags;

namespace Freem.Domain.UseCases.Contracts.Tags.Create;

public sealed class CreateTagResponse
{
    public Tag Tag { get; }

    public CreateTagResponse(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        Tag = tag;
    }
}