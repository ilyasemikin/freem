namespace Freem.Api.Public.Contracts;

/// <summary>Custom and well-known HTTP header names used by the API.</summary>
public static class HeaderNames
{
    /// <summary>Carries the bearer access token.</summary>
    public const string Authorization = "Authorization";
    /// <summary>Name of the calling client application.</summary>
    public const string ClientName = "X-CLIENT-NAME";
    
    /// <summary>Number of items in the returned page.</summary>
    public const string ItemsCount = "X-ITEMS-COUNT";
    /// <summary>Total number of items matching the query, regardless of paging.</summary>
    public const string TotalItemsCount = "X-TOTAL-ITEMS-COUNT";
}