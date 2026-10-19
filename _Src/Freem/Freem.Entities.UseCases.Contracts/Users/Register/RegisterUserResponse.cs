using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Users;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Register;

public sealed class RegisterUserResponse : IResponse<RegisterUserErrorCode>
{
    [MemberNotNullWhen(true, nameof(UserId))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public UserId? UserId { get; }
    public Error<RegisterUserErrorCode>? Error { get; }
    
    private RegisterUserResponse(
        UserId? userId = null, 
        Error<RegisterUserErrorCode>? error = null)
    {
        Success = userId is not null;
        UserId = userId;
        Error = error;
    }

    public static RegisterUserResponse CreateSuccess(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return new RegisterUserResponse(userId);
    }

    public static RegisterUserResponse CreateAlreadyRegisteredFailure(string message)
    {
        var error = new Error<RegisterUserErrorCode>(RegisterUserErrorCode.UserAlreadyRegistered, message);
        return new RegisterUserResponse(error: error);
    }

    public static RegisterUserResponse CreateUnknownFailure(string message)
    {
        var error = new Error<RegisterUserErrorCode>(RegisterUserErrorCode.UnknownError, message);
        return new RegisterUserResponse(error: error);
    }
}
