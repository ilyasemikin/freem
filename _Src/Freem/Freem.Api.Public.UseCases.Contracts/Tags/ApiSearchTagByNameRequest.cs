using Freem.Domain.Tags.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Tags;

public sealed class ApiSearchTagByNameRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.SearchByName;

    public TagName Name { get; }

    public ApiSearchTagByNameRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }
}
