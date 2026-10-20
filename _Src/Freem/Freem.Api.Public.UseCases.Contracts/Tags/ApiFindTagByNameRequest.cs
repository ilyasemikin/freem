using Freem.Domain.Tags.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiFindTagByNameRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.FindByName;

    public TagName Name { get; }

    public ApiFindTagByNameRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }
}
