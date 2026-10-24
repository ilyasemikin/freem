using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Activities.Remove;

public sealed class RemoveActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Remove;

    public ActivityId Id { get; }
    
    public RemoveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}