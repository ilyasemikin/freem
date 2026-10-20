using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Activities.Find;

public sealed class FindActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Find;

    public string SearchText { get; }
    
    public FindActivityRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}