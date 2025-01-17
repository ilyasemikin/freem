using Freem.Entities.Activities.Models;
using Freem.Entities.Common.Relations.Collections;

namespace Freem.Entities.UseCases.DTO.Activities.Create;

public sealed class CreateActivityRequest
{
    private readonly RelatedTagsCollection _tags = RelatedTagsCollection.Empty;
    
    public ActivityName Name { get; }

    public RelatedTagsCollection Tags
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