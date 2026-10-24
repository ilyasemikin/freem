using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Tags;

/// <summary>Result of creating a tag.</summary>
public sealed class TagCreateResponse
{
    /// <summary>Id of the created tag.</summary>
    public TagId Id { get; }
    
    public TagCreateResponse(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}