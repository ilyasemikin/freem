using Freem.Api.Public.Contracts.V1.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiListTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.List;

    public TagListRequest Query { get; }

    public ApiListTagRequest(TagListRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
