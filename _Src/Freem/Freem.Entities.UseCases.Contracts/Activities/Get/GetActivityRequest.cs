using Freem.Domain.Activities;

namespace Freem.Entities.UseCases.Contracts.Activities.Get;

public sealed class GetActivityRequest
{
    public ActivityId Id { get; }

    public GetActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}