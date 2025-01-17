using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.UseCases.DTO.Activities.Unarchive;

public sealed class UnarchiveActivityRequest
{
    public ActivityIdentifier Id { get; }
    
    public UnarchiveActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}