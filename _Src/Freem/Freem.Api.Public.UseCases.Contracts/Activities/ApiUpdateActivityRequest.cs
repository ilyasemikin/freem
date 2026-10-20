using Freem.Domain.Activities;
using Freem.Api.Public.Contracts.DTO.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiUpdateActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Update;

    public ActivityId Id { get; }
    public UpdateActivityRequest Body { get; }

    public ApiUpdateActivityRequest(ActivityId id, UpdateActivityRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}
