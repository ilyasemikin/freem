namespace Freem.Api.Public.Contracts.DTO.Activities;

/// <summary>Query for searching activities by name.</summary>
public sealed class FindActivityByNameRequest
{
    /// <summary>Text to look for in activity names. Must not be blank.</summary>
    public string SearchText { get; }

    public FindActivityByNameRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}