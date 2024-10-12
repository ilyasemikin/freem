using Freem.Entities.Tags;

namespace Freem.Entities.UseCases.Tags.Create.Models;

public sealed class CreateTagResponse
{
    public Tag Tag { get; }

    public CreateTagResponse(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        Tag = tag;
    }
}