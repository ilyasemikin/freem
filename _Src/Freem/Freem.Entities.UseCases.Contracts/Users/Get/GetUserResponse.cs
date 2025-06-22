using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Get;

public sealed class GetUserResponse : IResponse<GetUserErrorCode>
{
    [MemberNotNullWhen(true, nameof(User))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public User? User { get; }
    public Error<GetUserErrorCode>? Error { get; }

    private GetUserResponse(User? user = null, Error<GetUserErrorCode>? error = null)
    {
        Success = user is not null;
        User = user;
        Error = error;
    }

    public static GetUserResponse CreateSuccess(User user)
    {
        return new GetUserResponse(user);
    }

    public static GetUserResponse CreateNotFoundResult()
    {
        var error = new Error<GetUserErrorCode>(GetUserErrorCode.UserNotFound);
        return new GetUserResponse(error: error);
    }
}