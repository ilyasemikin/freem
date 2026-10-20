using Utilities.Errors;

namespace Freem.Web.Api.Public.Contracts.DTO;

public sealed class ErrorResponse
{
    public Error Error { get; }
    
    public ErrorResponse(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        
        Error = error;
    }
}