using Freem.Domain.Activities;

namespace Freem.Api.Public.Contracts.DTO.Activities;

/// <summary>Result of creating an activity.</summary>
public sealed class CreateActivityResponse
{
    /// <summary>Id of the created activity.</summary>
    public ActivityId Id { get; }

    public CreateActivityResponse(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}