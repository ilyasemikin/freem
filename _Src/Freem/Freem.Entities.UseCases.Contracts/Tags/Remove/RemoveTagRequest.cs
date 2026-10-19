using Freem.Domain.Tags;

namespace Freem.Entities.UseCases.Contracts.Tags.Remove;

public sealed class RemoveTagRequest
{
    public TagId Id { get; }

    public RemoveTagRequest(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}