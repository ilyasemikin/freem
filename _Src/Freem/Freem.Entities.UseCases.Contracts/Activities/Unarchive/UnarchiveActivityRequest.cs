using Freem.Entities.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Activities.Unarchive;

public sealed class UnarchiveActivityRequest
{
    public ActivityIdentifier Id { get; }
    
    public UnarchiveActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}