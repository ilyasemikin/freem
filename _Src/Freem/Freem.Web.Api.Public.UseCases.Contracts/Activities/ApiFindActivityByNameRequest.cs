using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiFindActivityByNameRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.FindByName;

    public FindActivityByNameRequest Query { get; }

    public ApiFindActivityByNameRequest(FindActivityByNameRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
