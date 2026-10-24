using Freem.Api.Public.Contracts.V1.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiSearchActivityByNameRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.SearchByName;

    public ActivitySearchByNameRequest Query { get; }

    public ApiSearchActivityByNameRequest(ActivitySearchByNameRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
