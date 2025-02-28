using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;

public sealed class AccessTokenValidationResult
{
    [MemberNotNullWhen(true, nameof(Properties))]
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsValid { get; }
    
    public AccessTokenProperties? Properties { get; }
    
    public Exception? Exception { get; }

    private AccessTokenValidationResult(
        bool isValid, 
        AccessTokenProperties? properties = null,
        Exception? exception = null)
    {
        IsValid = isValid;
        Properties = properties;
        Exception = exception;
    }
    
    public static AccessTokenValidationResult Valid(AccessTokenProperties properties)
    {
        return new AccessTokenValidationResult(true, properties);
    }

    public static AccessTokenValidationResult Invalid(Exception exception)
    {
        return new AccessTokenValidationResult(false, exception: exception);
    }
}