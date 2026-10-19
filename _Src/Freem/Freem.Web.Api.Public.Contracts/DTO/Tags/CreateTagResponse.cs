using Freem.Domain.Tags;

namespace Freem.Web.Api.Public.Contracts.DTO.Tags;

public sealed class CreateTagResponse
{
    public TagId Id { get; }
    
    public CreateTagResponse(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}