using Freem.Entities.Tags.Models;

namespace Freem.Entities.UseCases.Tags.Create.Models;

public sealed class CreateTagRequest
{
    public TagName Name { get; }

    public CreateTagRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }
}