using Freem.Entities.Tags.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Tags;

public sealed class CreateTagRequest
{
    public TagName Name { get; }

    public CreateTagRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
    }
}