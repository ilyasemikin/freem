using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Activities.Create;

public sealed class CreateActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Create;

    private readonly RelatedTagIdsCollection _tags = RelatedTagIdsCollection.Empty;
    
    public ActivityName Name { get; }
    public ActivityColor Color { get; }

    public RelatedTagIdsCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            
            _tags = value;
        }
    }

    public CreateActivityRequest(ActivityName name, ActivityColor color)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(color);
        
        Name = name;
        Color = color;
    }
}