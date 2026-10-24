using Freem.Api.Public.Contracts.V1.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiCreateTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Create;

    public TagCreateRequest Body { get; }

    public ApiCreateTagRequest(TagCreateRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
