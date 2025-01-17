using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.UseCases.DTO.Activities.Remove;

public sealed class RemoveActivityRequest
{
    public ActivityIdentifier Id { get; }
    
    public RemoveActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}