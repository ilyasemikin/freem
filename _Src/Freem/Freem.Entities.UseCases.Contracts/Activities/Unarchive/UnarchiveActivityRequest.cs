using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Activities.Unarchive;

public sealed class UnarchiveActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Unarchive;

    public ActivityId Id { get; }
    
    public UnarchiveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}