using Freem.Domain.Entities.Tags;

namespace Freem.Entities.UseCases.Contracts.Tags.Create;

public sealed class CreateTagResponse
{
    public Tag Tag { get; }

    public CreateTagResponse(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        Tag = tag;
    }
}