using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiListActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.List;

    public ListActivityRequest Query { get; }

    public ApiListActivityRequest(ListActivityRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
