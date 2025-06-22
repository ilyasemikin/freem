namespace Freem.Entities.UseCases.Contracts.Activities.Find;

public sealed class FindActivityRequest
{
    public string SearchText { get; }
    
    public FindActivityRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}