using Freem.Entities.Activities.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Activities;

public sealed class CreateActivityResponse
{
    public ActivityIdentifier Id { get; }

    public CreateActivityResponse(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}