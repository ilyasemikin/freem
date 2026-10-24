using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Tags.Search;

public sealed class SearchTagByNameRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.SearchByName;

    public string SearchText { get; }
    
    public SearchTagByNameRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}