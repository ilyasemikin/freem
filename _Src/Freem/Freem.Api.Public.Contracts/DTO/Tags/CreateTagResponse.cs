using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.DTO.Tags;

/// <summary>Result of creating a tag.</summary>
public sealed class CreateTagResponse
{
    /// <summary>Id of the created tag.</summary>
    public TagId Id { get; }
    
    public CreateTagResponse(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}