namespace Freem.Web.Api.Public.Contracts.DTO.Activities;

public sealed class FindActivityByNameRequest
{
    public string SearchText { get; }

    public FindActivityByNameRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}