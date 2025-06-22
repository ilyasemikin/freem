using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users.Identifiers;
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

    public static RegisterUserPasswordResponse CreateLoginAlreadyUsedFailure(string login)
    {
        var properties = new Dictionary<string, string>()
        {
            ["LOGIN"] = login
        };

        var error = new Error<RegisterUserPasswordErrorCode>(
            RegisterUserPasswordErrorCode.LoginAlreadyUsed,
            properties: properties);
        return new RegisterUserPasswordResponse(error: error);
    }

    public static RegisterUserPasswordResponse CreateUnknownFailure(string message)
    {
        var error = new Error<RegisterUserPasswordErrorCode>(RegisterUserPasswordErrorCode.UnknownError, message);
        return new RegisterUserPasswordResponse(error: error);
    }
}