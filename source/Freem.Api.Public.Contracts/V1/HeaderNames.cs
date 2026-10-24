namespace Freem.Api.Public.Contracts.V1;

/// <summary>Custom and well-known HTTP header names used by the API.</summary>
public static class HeaderNames
{
    /// <summary>Carries the bearer access token.</summary>
    public const string Authorization = "Authorization";
    /// <summary>Carries the file name of a downloaded file.</summary>
    public const string ContentDisposition = "Content-Disposition";
    /// <summary>Name of the calling client application.</summary>
    public const string ClientName = "X-CLIENT-NAME";
}