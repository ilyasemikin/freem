using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Identifiers;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Password.Register;

public sealed class RegisterUserPasswordResponse : IResponse<RegisterUserPasswordErrorCode>
{
    [MemberNotNullWhen(true, nameof(UserId))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public UserIdentifier? UserId { get; }
    public Error<RegisterUserPasswordErrorCode>? Error { get; }
    
    private RegisterUserPasswordResponse(
        UserIdentifier? userId = null, 
        Error<RegisterUserPasswordErrorCode>? error = null)
    {
        Success = userId is not null;
        UserId = userId;
        Error = error;
    }

    public static RegisterUserPasswordResponse CreateSuccess(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return new RegisterUserPasswordResponse(userId);
    }

    public static RegisterUserPasswordResponse CreateFailure(RegisterUserPasswordErrorCode code, string? message = null)
    {
        var error = new Error<RegisterUserPasswordErrorCode>(code, message);
        return new RegisterUserPasswordResponse(error: error);
    }
}