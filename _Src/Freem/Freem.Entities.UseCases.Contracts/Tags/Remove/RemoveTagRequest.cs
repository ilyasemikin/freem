using Freem.Domain.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Tags.Remove;

public sealed class RemoveTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Remove;

    public TagId Id { get; }

    public RemoveTagRequest(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}