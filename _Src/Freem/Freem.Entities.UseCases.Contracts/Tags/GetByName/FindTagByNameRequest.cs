using Freem.Entities.Tags.Models;

namespace Freem.Entities.UseCases.Contracts.Tags.GetByName;

public sealed class FindTagByNameRequest
{
    public string SearchText { get; }
    
    public FindTagByNameRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}