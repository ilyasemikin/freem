using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Domain.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiUpdateTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Update;

    public TagId Id { get; }
    public TagUpdateRequest Body { get; }

    public ApiUpdateTagRequest(TagId id, TagUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}
