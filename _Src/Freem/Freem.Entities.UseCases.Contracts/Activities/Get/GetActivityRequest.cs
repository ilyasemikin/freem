using Freem.Entities.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Activities.Get;

public sealed class GetActivityRequest
{
    public ActivityIdentifier Id { get; }

    public GetActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}