using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiRemoveActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Remove;

    public ActivityId Id { get; }

    public ApiRemoveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
