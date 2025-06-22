namespace Freem.Web.Api.Public.Contracts.DTO;

public sealed class ErrorResponse
{
    public string Code { get; }
    
    public string Message { get; }
    public IReadOnlyDictionary<string, string>? Properties { get; }
    
    public ErrorResponse(string code, string message, IReadOnlyDictionary<string, string>? properties = null)
    {
        Code = code;
        Message = message;
        Properties = properties;
    }
}