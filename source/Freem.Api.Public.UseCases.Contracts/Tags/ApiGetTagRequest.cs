using Freem.Domain.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiGetTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Get;

    public TagId Id { get; }

    public ApiGetTagRequest(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
