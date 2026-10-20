using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiCreateTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Create;

    public CreateTagRequest Body { get; }

    public ApiCreateTagRequest(CreateTagRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
