using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.UseCases.Tags.Get.Models;

public sealed class GetTagRequest
{
    public TagIdentifier Id { get; }

    public GetTagRequest(TagIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}