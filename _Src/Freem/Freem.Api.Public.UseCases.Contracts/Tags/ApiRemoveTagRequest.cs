using Freem.Domain.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiRemoveTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Remove;

    public TagId Id { get; }

    public ApiRemoveTagRequest(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
