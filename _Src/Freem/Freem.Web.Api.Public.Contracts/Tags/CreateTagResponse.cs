using Freem.Entities.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Tags;

public sealed class CreateTagResponse
{
    public TagIdentifier Id { get; }
    
    public CreateTagResponse(TagIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}