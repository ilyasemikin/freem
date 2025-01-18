using Freem.Entities.Tags.Models;

namespace Freem.Entities.UseCases.Contracts.Tags.Create;

public sealed class CreateTagRequest
{
    public TagName Name { get; }

    public CreateTagRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }
}