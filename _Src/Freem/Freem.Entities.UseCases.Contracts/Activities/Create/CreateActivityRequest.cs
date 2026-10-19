using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.Activities.Create;

public sealed class CreateActivityRequest
{
    private readonly RelatedTagIdsCollection _tags = RelatedTagIdsCollection.Empty;
    
    public ActivityName Name { get; }

    public RelatedTagIdsCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            
            _tags = value;
        }
    }

    public CreateActivityRequest(ActivityName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
    }
}