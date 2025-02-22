using Freem.Entities.Models.Activities;
using Freem.Entities.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.Activities.Create;

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