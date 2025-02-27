using Freem.Entities.Models.Tags;

namespace Freem.Entities.UseCases.Contracts.Tags.GetByName;

public sealed class GetTagByNameRequest
{
    public TagName Name { get; }
    
    public GetTagByNameRequest(TagName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
    }
}