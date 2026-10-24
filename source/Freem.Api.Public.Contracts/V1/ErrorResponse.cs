using Utilities.Errors;

namespace Freem.Api.Public.Contracts.V1;

/// <summary>Body of every error response.</summary>
public sealed class ErrorResponse
{
    /// <summary>Machine-readable error with its code and details.</summary>
    public Error Error { get; }
    
    public ErrorResponse(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        
        Error = error;
    }
}