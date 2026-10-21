using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Activities.Update;

public sealed class UpdateActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Update;

    public ActivityId Id { get; }
    
    public UpdateField<ActivityName>? Name { get; init; }

    public UpdateField<ActivityColor>? Color { get; init; }
    
    public UpdateField<RelatedTagIdsCollection>? Tags { get; init; }

    public UpdateActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    public bool HasChanges()
    {
        return Name is not null || Color is not null || Tags is not null;
    }
}