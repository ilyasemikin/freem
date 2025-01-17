using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.UseCases.DTO.Activities.Get;

public sealed class GetActivityRequest
{
    public ActivityIdentifier Id { get; }

    public GetActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}