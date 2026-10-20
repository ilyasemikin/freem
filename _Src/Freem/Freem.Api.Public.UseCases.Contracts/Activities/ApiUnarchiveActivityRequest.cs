using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiUnarchiveActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Unarchive;

    public ActivityId Id { get; }

    public ApiUnarchiveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
