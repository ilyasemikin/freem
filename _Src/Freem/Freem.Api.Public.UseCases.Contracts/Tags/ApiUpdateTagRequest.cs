using Freem.Domain.Tags;
using Freem.Api.Public.Contracts.DTO.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiUpdateTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Update;

    public TagId Id { get; }
    public UpdateTagRequest Body { get; }

    public ApiUpdateTagRequest(TagId id, UpdateTagRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}
