using System.Diagnostics.CodeAnalysis;

namespace Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;

public sealed class RefreshTokenValidationResult
{
    [MemberNotNullWhen(true, nameof(Properties))]
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsValid { get; }
    
    public RefreshTokenProperties? Properties { get; }
    
    public Exception? Exception { get; }

    private RefreshTokenValidationResult(bool isValid, RefreshTokenProperties? properties = null, Exception? exception = null)
    {
        IsValid = isValid;
        Exception = exception;
        Properties = properties;
    }

    public static RefreshTokenValidationResult Valid(RefreshTokenProperties properties)
    {
        ArgumentNullException.ThrowIfNull(properties);
        
        return new RefreshTokenValidationResult(true, properties);
    }

    public static RefreshTokenValidationResult Invalid(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        
        return new RefreshTokenValidationResult(false, exception: exception);
    }
}