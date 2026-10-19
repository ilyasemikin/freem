using Freem.Domain.Activities;

namespace Freem.Web.Api.Public.Contracts.DTO.Activities;

public sealed class CreateActivityResponse
{
    public ActivityId Id { get; }

    public CreateActivityResponse(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}