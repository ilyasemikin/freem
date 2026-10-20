using Freem.Api.Public.Contracts.DTO.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiListTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.List;

    public ListTagRequest Query { get; }

    public ApiListTagRequest(ListTagRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
