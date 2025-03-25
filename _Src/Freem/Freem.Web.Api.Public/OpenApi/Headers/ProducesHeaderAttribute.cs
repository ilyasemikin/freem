namespace Freem.Web.Api.Public.OpenApi.Headers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ProducesHeaderAttribute : Attribute
{
    public string HeaderName { get; }
    
    public int StatusCode { get; init; }

    public ProducesHeaderAttribute(string headerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(headerName);
        
        HeaderName = headerName;
    }
}