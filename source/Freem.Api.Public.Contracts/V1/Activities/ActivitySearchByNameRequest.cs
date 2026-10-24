namespace Freem.Api.Public.Contracts.V1.Activities;

/// <summary>Query for searching activities by name.</summary>
public sealed class ActivitySearchByNameRequest
{
    /// <summary>Text to look for in activity names. Must not be blank.</summary>
    public string SearchText { get; }

    public ActivitySearchByNameRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}