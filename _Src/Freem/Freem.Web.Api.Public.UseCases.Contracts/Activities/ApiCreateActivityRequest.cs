using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiCreateActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Create;

    public CreateActivityRequest Body { get; }

    public ApiCreateActivityRequest(CreateActivityRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
