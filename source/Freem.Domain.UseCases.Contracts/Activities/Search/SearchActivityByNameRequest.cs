using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Activities.Search;

public sealed class SearchActivityByNameRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Search;

    public string SearchText { get; }
    
    public SearchActivityByNameRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}