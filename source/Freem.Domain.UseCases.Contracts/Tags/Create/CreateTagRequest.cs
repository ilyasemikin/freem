using Freem.Domain.Tags.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Tags.Create;

public sealed class CreateTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Create;

    public TagName Name { get; }

    public CreateTagRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }
}