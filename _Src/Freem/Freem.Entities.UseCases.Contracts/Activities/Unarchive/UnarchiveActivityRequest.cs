using Freem.Domain.Activities;

namespace Freem.Entities.UseCases.Contracts.Activities.Unarchive;

public sealed class UnarchiveActivityRequest
{
    public ActivityId Id { get; }
    
    public UnarchiveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}