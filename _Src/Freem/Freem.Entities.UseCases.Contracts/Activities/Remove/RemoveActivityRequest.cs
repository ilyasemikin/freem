using Freem.Domain.Activities;

namespace Freem.Entities.UseCases.Contracts.Activities.Remove;

public sealed class RemoveActivityRequest
{
    public ActivityId Id { get; }
    
    public RemoveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}