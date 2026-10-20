using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.DTO.Tags;

public sealed class CreateTagRequest
{
    public TagName Name { get; }

    public CreateTagRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
    }
}