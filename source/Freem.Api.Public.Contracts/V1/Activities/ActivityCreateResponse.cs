using Freem.Domain.Activities;

namespace Freem.Api.Public.Contracts.V1.Activities;

/// <summary>Result of creating an activity.</summary>
public sealed class ActivityCreateResponse
{
    /// <summary>Id of the created activity.</summary>
    public ActivityId Id { get; }

    public ActivityCreateResponse(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}